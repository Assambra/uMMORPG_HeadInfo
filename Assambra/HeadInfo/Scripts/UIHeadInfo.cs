using UnityEngine;
using UnityEngine.UI;

public class UIHeadInfo : MonoBehaviour
{
    public Entity thisEntity;
    public GameObject healthBar;
    public Slider healthSlider;
    public RectTransform rectTransform;
    public Image targetImage;
    public Sprite targetSprite;

    public bool attackMode = false;
    public bool selectMode = false;
    public bool isPlayer = false;

    private bool isFullVisible;
    private NetworkManagerMMO networkManagerMMO;

    private void Start()
    {
        networkManagerMMO = FindObjectOfType<NetworkManagerMMO>();

        targetImage.sprite = null;
        targetImage.color = Color.clear;
    }

    void Update ()
    {
        if(networkManagerMMO.state == NetworkState.Lobby)
        {
            // Face the Panel to the Camera,  without a isFullVisible check 
            transform.forward = Camera.main.transform.forward;
            // Also set healthbar inactive because if we do this in the Start() function it get problems with the Healtbar position (maybe a Content Size Fitter problem).
            // The Problem occours if the Player spawned on the Server and select a target then the health bar position is wrong; 
            if (healthBar.activeSelf)
            {
                healthBar.SetActive(false);
            }
        }

        Player player = Player.localPlayer;
        if (!player) return;

        // I want to use FaceCamera.cs from uMMORPG but it dosn`t detect the Canvas Renderer
        // So we use this RenderExtension founded on:  https://forum.unity.com/threads/test-if-ui-element-is-visible-on-screen.276549/#post-2978773
        // These renderer extensions were originally created by KGS. We are allowed to use it with the kind permission of the author KGS in our project.
        // Todo Do this check in a Coroutine, not every frame;
        isFullVisible = rectTransform.IsVisibleFrom(Camera.main);

        if(isPlayer)
        {
            healthBar.SetActive(true);
            healthSlider.value = thisEntity.HealthPercent();
        }

        if (selectMode)
        {
            targetImage.sprite = targetSprite;

            if(!isPlayer)
            {
                healthBar.SetActive(true);
                healthSlider.value = thisEntity.HealthPercent();
            }

            if (attackMode)
                targetImage.color = Color.red;
            else
                targetImage.color = Color.white;
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

    private void Clear()
    {
        targetImage.sprite = null;
        targetImage.color = Color.clear;
        if(!isPlayer)
            healthBar.SetActive(false);
    }
}
