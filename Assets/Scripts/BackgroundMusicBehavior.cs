using UnityEngine;

public class BackgroundMusicBehavior : MonoBehaviour
{
    public UIController UIController;
    public AudioClip AmbianceSound;
    public AudioClip MenuSound;

    private AudioSource _audioSource;

    private bool _isUIActive;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_isUIActive != UIController.IsActiveAny())
        {
            if (UIController.IsActiveAny())
            {
                _audioSource.clip = MenuSound;
            }
            else
            {
                _audioSource.clip = AmbianceSound;
            }
            _audioSource.Play();
            _isUIActive = UIController.IsActiveAny();
        }
    }
}
