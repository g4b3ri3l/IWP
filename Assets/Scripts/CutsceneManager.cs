using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    [SerializeField] private PlayableDirector playableDirector;

    public void HandleUpdate()
    {
        if (playableDirector.state != PlayState.Playing)
        {
            GameController.Instance.state = GameState.FreeRoam;
        }
    }
}
