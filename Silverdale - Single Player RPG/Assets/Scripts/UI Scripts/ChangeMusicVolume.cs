using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicVolume : MonoBehaviour {

    public Slider volume;
    public AudioSource myMusic;

	void Update ()
    {
        myMusic.volume = volume.value;
	}
}
