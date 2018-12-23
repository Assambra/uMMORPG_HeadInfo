using UnityEngine;
using UnityEngine.UI;

public class UIHeadInfo : MonoBehaviour
{
    public Entity thisEntity;
    public GameObject headInfoPanel;
    public GameObject entityNamePrefab;
    public GameObject guildNamePrefab;
    public GameObject healthBarPrefab;
    public Image headInfoPanelImage;
    public Sprite targetSprite;

    /// <summary>
    /// The name of the Entity
    /// </summary>
    public string EntityName { set { entityName = value; } }
    /// <summary>
    /// The guild name of the entity
    /// </summary>
    public string GuildName { set { guildName = value; } }
    /// <summary>
    /// Sets the select mode and changes the panel image to targetSprite and set headInfoPanelImage color to white
    /// </summary>
    public bool SelectMode { set { selectMode = value; } }
    /// <summary>
    /// Sets the attack mode and changes the headInfoPanelImage color to red
    /// </summary>
    public bool AttackMode { set { attackMode = value; } }
    /// <summary>
    /// Set this to true for a local player
    /// </summary>
    public bool IsPlayer { set { isPlayer = value; } }
    /// <summary>
    /// Whether the player health bar should always be displayed, default true
    /// </summary>
    public bool AlwaysShowPlayerHealth { set { alwaysShowPlayerHealth = value; } }

    private string entityName = "";
    private string guildName = "";
    private bool attackMode = false;
    private bool selectMode = false;
    private bool isPlayer = false;
    private bool alwaysShowPlayerHealth = true;

    private NetworkManagerMMO networkManagerMMO;
    private RectTransform headInfoPanelRectTransform;
    private bool isFullVisible;

    private GameObject goEntityName;
    private GameObject goGuildName;
    private GameObject goHealthBar;

    private Text entityNameText;
    private Text guildNameText;
    private Slider healthBarSlider;

    private void Start()
    {
        networkManagerMMO = FindObjectOfType<NetworkManagerMMO>();
        headInfoPanelRectTransform = headInfoPanel.GetComponent<RectTransform>();

        headInfoPanelImage.sprite = null;
        headInfoPanelImage.color = Color.clear;

        goEntityName = InstantiateHeadInfoPrefab(entityNamePrefab, headInfoPanel.transform);
        entityNameText = goEntityName.GetComponent<Text>();
        goGuildName = InstantiateHeadInfoPrefab(guildNamePrefab, headInfoPanel.transform);
        guildNameText = goGuildName.GetComponent<Text>();
        goHealthBar = InstantiateHeadInfoPrefab(healthBarPrefab, headInfoPanel.transform);
        healthBarSlider = goHealthBar.GetComponent<Slider>();
    }

    void Update ()
    {
        if(networkManagerMMO.state == NetworkState.Lobby)
        {
            // Face the Panel to the Camera,  without a isFullVisible check 
            transform.forward = Camera.main.transform.forward;

            goGuildName.SetActive(false);
            goHealthBar.SetActive(false);

            entityNameText.text = entityName;
        }

        Player player = Player.localPlayer;
        if (!player) return;

        // I want to use FaceCamera.cs from uMMORPG but it dosn`t detect the Canvas Renderer
        // So we use this RenderExtension founded on:  https://forum.unity.com/threads/test-if-ui-element-is-visible-on-screen.276549/#post-2978773
        // These renderer extensions were originally created by KGS. We are allowed to use it with the kind permission of the author KGS in our project.
        // Todo Do this check in a Coroutine, not every frame;
        isFullVisible = headInfoPanelRectTransform.IsVisibleFrom(Camera.main);

        entityNameText.text = entityName;

        if(guildName != "")
        {
            goGuildName.SetActive(true);
            guildNameText.text = guildName;
        }
        else
        {
            goGuildName.SetActive(false);
        }

        if(isPlayer && alwaysShowPlayerHealth)
        {
            goHealthBar.SetActive(true);
            healthBarSlider.value = thisEntity.HealthPercent();
        }

        if (selectMode)
        {
            headInfoPanelImage.sprite = targetSprite;

            if(!isPlayer || !alwaysShowPlayerHealth)
            {
                goHealthBar.SetActive(true);
                healthBarSlider.value = thisEntity.HealthPercent();
            }

            if (attackMode)
                headInfoPanelImage.color = Color.red;
            else
                headInfoPanelImage.color = Color.white;
        }
        else
            Clear();
    }

    private void LateUpdate()
    {
        if (isFullVisible)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    /// <summary>
    /// Reset the HeadInfo Panel to default
    /// </summary>
    private void Clear()
    {
        headInfoPanelImage.sprite = null;
        headInfoPanelImage.color = Color.clear;
        if(!isPlayer || !alwaysShowPlayerHealth)
            goHealthBar.SetActive(false);
    }

    private GameObject InstantiateHeadInfoPrefab(GameObject prefab, Transform parent)
    {
        GameObject go = Instantiate(prefab, parent);

        return go;
    }
}
