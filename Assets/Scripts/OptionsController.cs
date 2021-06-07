using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public GameController GameController; 
    public SettingsController _settingsController;

    public Toggle SoundsToggle;
    public Toggle VibrationToggle;
    public Slider DifficultySlider;

    public int? ReturnTo { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        UpdateFromSettings();
    }

    public void BtnOK()
    {
        _settingsController.SoundsOn = SoundsToggle.isOn;
        _settingsController.PlayVsComputer = VibrationToggle.isOn;
        _settingsController.Difficulty = (int)DifficultySlider.value;
        Debug.Log("Saved");

        if (SoundsToggle.isOn)
        {
            AudioListener.volume = 1f;
        }
        else
        {
            AudioListener.volume = 0f;
        }

        if (ReturnTo.HasValue)
            GameController.PauseAndShow(ReturnTo.Value);
        else
            GameController.PauseAndShow(2);

        ReturnTo = null;
    }

    public void UpdateFromSettings()
    {
        SoundsToggle.isOn = _settingsController.SoundsOn;
        VibrationToggle.isOn = _settingsController.PlayVsComputer;
        DifficultySlider.value = _settingsController.Difficulty;
    }
}
