using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        #if(!UNITY_EDITOR)
        SceneManager.LoadScene(1);
        #endif
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            {
            #if(UNITY_EDITOR)
            if(Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                SceneManager.LoadScene(1);
            }
            if(Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                SceneManager.LoadScene(2);
            }
            #endif
        }
    }

    public void LoadMap(int mapID, Vector3 position = new Vector3())
    {
        StartCoroutine(LoadMap_C(mapID, position));
    }

    //should take parameters like map index, player rotation, etc.
    //we can most likely add animations too, no clue how they work with coroutines though
    public IEnumerator LoadMap_C(int mapID, Vector3 position)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield break;
    }
}
