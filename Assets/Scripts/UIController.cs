using System;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject Options;
    public GameObject AreYouSureToRestart;
    public GameObject PlayPause;
    public GameObject Start_;
    public GameObject GameOver;

    public GameObject[] HideObjectWhenActive;

    public UnityEvent UIChanged;

    void Start()
    {
        DeactivateAll();
    }

    public int ActivePanelId()
    {
        return GetComponent<Animator>().GetInteger("PanelId");
    }

    public void Activate(int panelId)
    {
        Activate((UIPanels)panelId);
    }

    public void Activate(UIPanels panel)
    {
        GetComponent<Animator>().SetInteger("PanelId", (int)panel);
        SetHiddensActive(false);

        if (UIChanged != null)
            UIChanged.Invoke();
    }

    public void DeactivateAll()
    {
        GetComponent<Animator>().SetInteger("PanelId", -1);
        SetHiddensActive(true);

        if (UIChanged != null)
            UIChanged.Invoke();
    }

    private void SetHiddensActive(bool value)
    {
        if(HideObjectWhenActive != null)
        {
            foreach (var item in HideObjectWhenActive)
            {
                item.SetActive(value);
            }
        }
    }

    public bool IsActiveAny()
    {
        return ActivePanelId() != -1;
    }

    public enum UIPanels : int
    {
        Options,
        AreYouSureToRestart,
        PlayPause,
        Start,
        GameOver
    }
}
