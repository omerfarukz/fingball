using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ToogleAnimator : MonoBehaviour
{
    public Animator Animator;
    public Toggle Toogle;

    void Start()
    {
        toogleValueChanged(Toogle.isOn);
        Toogle.onValueChanged.AddListener(toogleValueChanged);
    }

    private void Update()
    {
        if (Animator.GetBool("isOn") != Toogle.isOn)
            updateAnimation();
    }

    public void updateAnimation()
    {
        Animator.SetBool("isOn", Toogle.isOn);
    }

    public void toogleValueChanged(bool value)
    {
        updateAnimation();
        if (value)
            ToogleOn.Invoke();
        else
            ToogleOff.Invoke();
    }

    public UnityEvent ToogleOn;
    public UnityEvent ToogleOff;
}