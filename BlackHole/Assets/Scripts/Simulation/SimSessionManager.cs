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
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        MainMenuUI.OnStartPressed += StartPreparing;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        MainMenuUI.OnStartPressed -= StartPreparing;

    }

    private void StartPreparing(int lYear, int rYear)
    {
        DataManager.ins.SetupStreaming(lYear, rYear);

        SceneLoader.LoadScene(SceneName.LoadingScreen);
    }

    void OnSceneLoad(Scene s, LoadSceneMode lsm)
    {
        if (s.name == "simulation")
        {
            SimManager.ins.PrepareSimulation();
        }
        if (s.name == "loading")
        {
            DataManager.ins.StartStreaming();
        }
        if (s.name == "mainmenu")
        {
            DataManager.ins.ResetStreaming();
        }
    }
}
