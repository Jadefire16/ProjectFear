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
    [Tooltip("Will stop the game and load the next scene, not advised unless that is the desired behaviour")]
    [SerializeField] private bool loadSceneStatic = false;
    [Tooltip("Will load the scene as a new scene layer, allowing multiple scenes at once")]
    [SerializeField] private bool loadSceneAdditively = false;


    public void LoadScene()
    {
        switch (loadBehaviour)
        {
            case LoadBehaviour.Load:
                if(!loadSceneStatic)
                    SceneHandler.Manager.LoadSceneAsync(sceneIndex, loadSceneAdditively ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single);
                else
#pragma warning disable CS0618 // Type or member is obsolete
                    SceneHandler.Manager.LoadSceneStatic(sceneIndex);
#pragma warning restore CS0618 // Type or member is obsolete
                break;
            case LoadBehaviour.Unload:
                SceneHandler.Manager.UnloadSceneAsync(sceneIndex);
                break;
        }
    }
}
