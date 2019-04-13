using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SimSessionManager : MonoBehaviour
{
    public static SimSessionManager ins;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        } else if (ins != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoad;
        // SimManager.OnSimEnd += HandleSimEnd;
    }

    void OnSceneLoad(Scene s, LoadSceneMode lsm)
    {
        if (s.name == "simulation")
        {
            // SimManager.StartSim();
        }
        if (s.name == "loading")
        {

        }
    }

    void HandleSimEnd(bool simCompleted)
    {
        SceneLoader.LoadScene(SceneName.MainMenu);
    }

}
