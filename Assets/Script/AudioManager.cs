using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioSource boxShakeSource;

    public AudioClip[] audioList;

    public void SetAudio(string _name)
    {
        int index = 0;
        switch (_name)
        {
            case "Drop": index = 0; break;
            case "Merge": index = 1; break;
            case "Button": index = 2; break;
            case "Thunder": index = 3; break;
            case "Over": index = 4; break;
            case "Exit": index = 5; break;
        }
        effectSource.clip = audioList[index];
        effectSource.Play();
    }

    public void SetBGM(float _value)
    {
        bgmSource.volume += _value;
    }

    public void SetSFX(float _value)
    {
        effectSource.volume += _value;
        boxShakeSource. volume += _value;
    }

    public void BgmStop()
    {
        bgmSource.Stop();
    }
    public void BoxAudioPlay()
    {
        boxShakeSource.Play();
    }

}
