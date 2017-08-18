using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Audio[] sounds;

    public static AudioManager currentInstance;

	// Use this for initialization
	void Awake () {
        if (currentInstance == null)
            currentInstance = this;
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

		foreach(Audio a in sounds)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
	}
	
	public void Play(string name)
    {
        Audio s = Array.Find(sounds, Audio => Audio.name == name);
        if(s == null)
        {
            Debug.LogWarning("No audio named: " + name + "!");
        }
    }
}
