using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Renderers")]
    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer leftHandRenderer;
    public SpriteRenderer rightHandRenderer;
    public SpriteRenderer shieldRenderer;
    public SpriteRenderer hairRenderer;

    void Start()
    {
        leftHandRenderer.sprite = null;
        rightHandRenderer.sprite = null;
        shieldRenderer.sprite = null;
    }

    // HEAD
    public void SetHead(Sprite sprite, bool hideHair)
    {
        if (headRenderer != null)
            headRenderer.sprite = sprite;

        if (hairRenderer != null)
            hairRenderer.enabled = !hideHair;
    }

    // BODY
    public void SetBody(Sprite sprite)
    {
        if (bodyRenderer != null)
            bodyRenderer.sprite = sprite;
    }

    // HANDS / WEAPONS
    public void SetLeftHand(Sprite sprite)
    {
        if (leftHandRenderer != null)
            leftHandRenderer.sprite = sprite;
    }

    public void SetRightHand(Sprite sprite)
    {
        if (rightHandRenderer != null)
            rightHandRenderer.sprite = sprite;
    }

    public void SetShield(Sprite sprite)
    {
        if (shieldRenderer != null)
            shieldRenderer.sprite = sprite;
    }
}