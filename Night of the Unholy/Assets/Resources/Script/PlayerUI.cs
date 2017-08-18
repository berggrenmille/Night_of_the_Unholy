using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public enum GameState
    {
        PLAYING, PAUSED, DEAD
    }
    public Canvas PausedMenu;
    public Canvas IngameMenu;
    public Canvas DeathMenu;

    public GameState state;

    // Use this for initialization
    void Start()
    {
        state = GameState.PLAYING;
        CheckState();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePause()
    {
        if(state == GameState.PLAYING)
        {
            state = GameState.PAUSED;
        }
        else if(state == GameState.PAUSED)
        {
            state = GameState.PLAYING;    
        }
        CheckState();
    }

    void CheckState()
    {
        switch (state)
        {
            case GameState.PLAYING:
                IngameMenu.gameObject.SetActive(true);
                PausedMenu.gameObject.SetActive(false);
                DeathMenu.gameObject.SetActive(false);
                break;
            case GameState.PAUSED:
                IngameMenu.gameObject.SetActive(false);
                PausedMenu.gameObject.SetActive(true);
                DeathMenu.gameObject.SetActive(false);
                break;
            case GameState.DEAD:
                IngameMenu.gameObject.SetActive(false);
                PausedMenu.gameObject.SetActive(false);
                DeathMenu.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
