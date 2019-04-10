using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneLoader
{
   // Load scene based on enum
   public static void LoadScene(SceneName name)
   {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)name);
    }
}

public enum SceneName
{
    MainMenu,
    Simulation
}