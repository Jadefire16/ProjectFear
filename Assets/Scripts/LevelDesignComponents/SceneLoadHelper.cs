using JadesToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadHelper : MonoBehaviour
{

    [Header("Required")]
    [Space]
    [SerializeField] private int sceneIndex;
    [Space]
    [SerializeField] private LoadBehaviour loadBehaviour;
    [SerializeField] private bool loadSceneAdditively = false;


    public void LoadScene()
    {
        switch (loadBehaviour)
        {
            case LoadBehaviour.Load:
                SceneHandler.Manager.LoadSceneAsync(sceneIndex, loadSceneAdditively ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single);
                break;
            case LoadBehaviour.Unload:
                SceneHandler.Manager.UnloadSceneAsync(sceneIndex);
                break;
        }
    }
}
