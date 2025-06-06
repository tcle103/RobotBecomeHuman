using UnityEngine;

public class ColorblindSprites : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite protanopiaSprite;
    public Sprite deuteranopiaSprite;

    void Start()
    { 
        if (PlayerPrefs.GetInt("ToggleBool2") == 1)
        {
            spriteRenderer.sprite = protanopiaSprite;
        }
        if (PlayerPrefs.GetInt("ToggleBool3") == 1)
        {
            spriteRenderer.sprite = deuteranopiaSprite;
        }
    }

}