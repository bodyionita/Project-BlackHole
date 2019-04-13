using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
    private void LoadEnd()
    {
        SceneLoader.LoadScene(SceneName.Simulation);
    }

    private void OnEnable()
    {
        loadingtext.OnLoadingDone += LoadEnd;
    }

    private void OnDisable()
    {
        loadingtext.OnLoadingDone -= LoadEnd;
    }
}
