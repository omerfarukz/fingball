using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameController GameController;

    private void OnMouseUp()
    {
        if(!GameController.UIController.IsActiveAny())
            GameController.PauseAndShow((int)UIController.UIPanels.PlayPause);
    }
}
