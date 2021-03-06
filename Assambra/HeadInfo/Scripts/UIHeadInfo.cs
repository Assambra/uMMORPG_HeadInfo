﻿using UnityEngine;
using UnityEngine.UI;

public class UIHeadInfo : MonoBehaviour
{
    // Entity
    public Entity thisEntity;
    // Automatic head info position
    public float adjustmentHeadInfoPositionY = 0.4f;
    public GameObject model3D;
    //Panel stuff
    public GameObject headInfoPanel;
    public Image headInfoPanelImage;
    public Sprite targetSprite;
    // Prefabs
    public GameObject questSignPrefab;
    public GameObject stunnedPrefab;
    public GameObject entityNamePrefab;
    public GameObject guildNamePrefab;
    public GameObject healthBarPrefab;

    /// <summary>
    /// The quest sign for npc entities
    /// </summary>
    public string QuestSign { set { questSign = value; } }
    /// <summary>
    /// Entity stunned
    /// </summary>
    public bool IsStunned { set { isStunned = value; } } 
    /// <summary>
    /// The name of the Entity
    /// </summary>
    public string EntityName { set { entityName = value; } }
    /// <summary>
    /// To change the color of the entities name
    /// </summary>
    public Color EntityNameColor { set { entityNameColor = value; } }
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
    public bool IsLocalPlayer { set { isLocalPlayer = value; } }
    /// <summary>
    /// Set this to true for a npc entity
    /// </summary>
    public bool IsNpc { set { isNpc = value; } }
    /// <summary>
    /// Set this to true for a mount entity
    /// </summary>
    public bool IsMount { set { isMount = value; } }
    /// <summary>
    /// Whether the player health bar should always be displayed, default true
    /// </summary>
    public bool AlwaysShowHealth { set { alwaysShowHealth = value; } }

    private string questSign = "";
    private bool isStunned = false;
    private string entityName = "";
    private Color entityNameColor = Color.white;
    private string guildName = "";
    private bool selectMode = false;
    private bool attackMode = false;
    private bool isLocalPlayer = false;
    private bool isNpc = false;
    private bool isMount = false;
    private bool alwaysShowHealth = false;

    private NetworkManagerMMO networkManagerMMO;
    private RectTransform headInfoPanelRectTransform;
    private bool isFullVisible;

    private GameObject goQuestSign;
    private GameObject goStunned;
    private GameObject goEntityName;
    private GameObject goGuildName;
    private GameObject goHealthBar;

    private Text questSignText;
    private Text entityNameText;
    private Text guildNameText;
    private Slider healthBarSlider;

    private GameObject canvasHeadInfo;
    private CapsuleCollider capsuleCollider;
    private float scaleY;

    private void Start()
    {
        canvasHeadInfo = gameObject.transform.parent.gameObject;
        capsuleCollider = thisEntity.collider.GetComponent<CapsuleCollider>();
        networkManagerMMO = FindObjectOfType<NetworkManagerMMO>();
        headInfoPanelRectTransform = headInfoPanel.GetComponent<RectTransform>();

        headInfoPanelImage.sprite = null;
        headInfoPanelImage.color = Color.clear;
        if (isNpc)
        {
            goQuestSign = InstantiateHeadInfoPrefab(questSignPrefab, headInfoPanel.transform.parent.gameObject.transform);
            questSignText = goQuestSign.GetComponent<Text>();
        }
        goStunned = InstantiateHeadInfoPrefab(stunnedPrefab, headInfoPanel.transform.parent.gameObject.transform);
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
            // Set the Y position of the HeadInfo panel
            if(!isMount)
            {
                scaleY = model3D.transform.localScale.y;
                float headInfoPosition = (capsuleCollider.height + adjustmentHeadInfoPositionY) * scaleY;
                canvasHeadInfo.transform.position = new Vector3(thisEntity.transform.position.x, thisEntity.transform.position.y + headInfoPosition, thisEntity.transform.position.z);
            }
            
            // Face the Panel to the Camera,  without a isFullVisible check 
            transform.forward = Camera.main.transform.forward;

            if (isMount)
                goEntityName.SetActive(false);

            goStunned.SetActive(false);
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

        // Set the Y position of the HeadInfo panel
        if(!isMount)
        {
            if (scaleY != thisEntity.transform.localScale.y)
            {
                scaleY = model3D.transform.localScale.y;
                float headInfoPosition = (capsuleCollider.height + adjustmentHeadInfoPositionY) * scaleY;
                canvasHeadInfo.transform.position = new Vector3(thisEntity.transform.position.x, thisEntity.transform.position.y + headInfoPosition, thisEntity.transform.position.z);
            }
        }

        if (isNpc)
            questSignText.text = questSign;

        goStunned.SetActive(isStunned);

        
            entityNameText.text = entityName;
            entityNameText.color = entityNameColor;
       
        
        if (guildName != "")
        {
            if(!goGuildName.activeSelf)
                goGuildName.SetActive(true);

            guildNameText.text = guildName;
        }
        else
        {
            if (goGuildName.activeSelf)
                goGuildName.SetActive(false);
        }

        if (alwaysShowHealth)
        {
            goHealthBar.SetActive(true);
            healthBarSlider.value = thisEntity.HealthPercent();
        }

        if (selectMode)
        {
            headInfoPanelImage.sprite = targetSprite;

            if (isMount)
                goEntityName.SetActive(true);

            if (!alwaysShowHealth)
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
        if (isMount)
            goEntityName.SetActive(false);

        headInfoPanelImage.sprite = null;
        headInfoPanelImage.color = Color.clear;

        if (!alwaysShowHealth)
            goHealthBar.SetActive(false);
    }

    private GameObject InstantiateHeadInfoPrefab(GameObject prefab, Transform parent)
    {
        GameObject go = Instantiate(prefab, parent);

        return go;
    }
}
