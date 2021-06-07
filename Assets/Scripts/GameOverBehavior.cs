using UnityEngine;
using UnityEngine.UI;

public class GameOverBehavior : MonoBehaviour
{
    public GameController GameController;
    public Image WinLose;
    public Sprite YouLose;
    public Sprite YouWin;
    public SettingsController SettingsController;

    private void OnEnable()
    {
        GetReady(GameController.PlayerWin);
    }

    public void GetReady(bool playerWins)
    {
        if (playerWins)
        {
            WinLose.sprite = YouWin;
            SetRotation();
        }
        else if (!playerWins)
        {
            if(SettingsController.PlayVsComputer)
                WinLose.sprite = YouLose;
            else
                WinLose.sprite = YouWin;

            SetRotation();
        }
    }

    private void SetRotation()
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (!SettingsController.PlayVsComputer && !GameController.PlayerWin)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
}