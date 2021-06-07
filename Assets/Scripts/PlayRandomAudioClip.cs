using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomAudioClip : MonoBehaviour
{
    public AudioClip[] AudioClips;

    void Start()
    {
        if(AudioClips != null) {
            int randomIndex = Random.Range(0, AudioClips.Length - 1);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = AudioClips[randomIndex];
            audioSource.Play();
//            Debug.Log(audioSource.clip.name);
        }
    }
}
