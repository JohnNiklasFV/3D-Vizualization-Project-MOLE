using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public Image icon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool soundOn = true;

    void Start()
    {
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;

        ApplySoundState();
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;

        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);

        ApplySoundState();
    }

    private void ApplySoundState()
    {
        AudioListener.volume = soundOn ? 1f : 0f;

        if (icon != null)
        {
            icon.sprite = soundOn ? soundOnSprite : soundOffSprite;
        }
    }
}
