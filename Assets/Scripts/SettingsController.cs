using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public bool SoundsOn
    {
        get
        {
            return PlayerPrefs.GetInt("SoundsOn", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("SoundsOn", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool PlayVsComputer
    {
        get
        {
            return PlayerPrefs.GetInt("PlayVsComputer", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("PlayVsComputer", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public int Difficulty
    {
        get
        {
            return PlayerPrefs.GetInt("Difficulty", 2);
        }
        set
        {
            PlayerPrefs.SetInt("Difficulty", value);
            PlayerPrefs.Save();
        }
    }

    public int PlayCount
    {
        get
        {
            return PlayerPrefs.GetInt("PlayCount", 0);
        }
        set
        {
            PlayerPrefs.SetInt("PlayCount", value);
            PlayerPrefs.Save();
        }
    }
}