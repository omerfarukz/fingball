using UnityEngine;

public class StartBehavior : MonoBehaviour
{
    public GameController GameController;
    public UIController UIController;
    public OptionsController OptionsController;

    public void Setting()
    {
        OptionsController.ReturnTo = 3;
        GameController.PauseAndShow(0);
    }

    private void SettingInternal()
    {

    }

    public void Share()
    {
        var ns = new NativeShare();
        ns.SetText("Fingball http:///smarturl.it/fingball");
        ns.Share();
    }

    public void Rate()
    {
        Application.OpenURL("http://smarturl.it/fingball_rate");
    }
}
