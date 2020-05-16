using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void setLevel(float sliderVal)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
    }
}
