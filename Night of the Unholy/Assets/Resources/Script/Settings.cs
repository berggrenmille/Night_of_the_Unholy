using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    public static Settings currentInstance;
    private Settings()
    { }

    //variables here
    [System.Serializable]
    public class MouseSettings
    {
        public float sensitivity = 1.0f;
        public float smoothness = 1.0f;

        public bool inverted = false;
        public bool raw = true;
    }

    public MouseSettings mouseSettings = new MouseSettings();

    private void Setup()
    {
        //Get and set all settings from and to file
        mouseSettings.sensitivity = PlayerPrefs.GetFloat("m_sensitivity", mouseSettings.sensitivity);
        mouseSettings.smoothness = PlayerPrefs.GetFloat("m_smoothness", mouseSettings.smoothness);
        mouseSettings.inverted = Convert.ToBoolean(PlayerPrefs.GetInt("m_inverted", Convert.ToInt32(mouseSettings.inverted)));
        mouseSettings.raw = Convert.ToBoolean(PlayerPrefs.GetInt("m_raw", Convert.ToInt32(mouseSettings.raw)));

        PlayerPrefs.Save();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("m_sensitivity", mouseSettings.sensitivity);
        PlayerPrefs.SetFloat("m_smoothness", mouseSettings.smoothness);
        PlayerPrefs.SetInt("m_inverted", Convert.ToInt32(mouseSettings.inverted));
        PlayerPrefs.SetInt("m_raw", Convert.ToInt32(mouseSettings.raw));

        PlayerPrefs.Save();
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        Setup();
    }

    public void UpdateSettings(Player _player)
    {
        //UpdateSettingsHere
    }

    public static Settings GetInstance()
    {
        if(currentInstance == null)
        {
            GameObject go = new GameObject();
            go.name = "Settings instance";
            currentInstance = go.AddComponent<Settings>();
            currentInstance.Setup();
        }
        return currentInstance;
    }

}
