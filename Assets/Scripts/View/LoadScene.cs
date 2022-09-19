using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour
{
    public CanvasGroup canvas;
    public Camera camera;
    private Scene currentScene;

    private void Start()
    {
        //LoadThisScene(1);
        ButtonLoadScene.OnLoadButtonClicked += LoadThisScene;
    }

    private void OnDisable()
    {
        ButtonLoadScene.OnLoadButtonClicked -= LoadThisScene;
    }


    public void LoadThisScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator DescativateButtons(int sceneIndex)
    {
        foreach (EventSystem es in FindObjectsOfType<EventSystem>())
        {
            es.gameObject.SetActive(false);
        }

        yield return LoadSceneAsync(sceneIndex);

        foreach (EventSystem es in FindObjectsOfType<EventSystem>())
        {
            es.gameObject.SetActive(true);
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 0f;

        while (canvas.alpha < 1f)
        {
            canvas.alpha += Time.deltaTime;
            yield return null;
        }

        

        foreach (Camera c in Camera.allCameras)
        { if (c != camera) { c.gameObject.SetActive(false); } }
        camera.gameObject.SetActive(true);

        if (currentScene.isLoaded) {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
            while (!unloadOperation.isDone)
            { yield return null; }
        }
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            { yield return null; }
        }

        while (SceneManager.sceneCount > 2)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(2));
        }

        camera.gameObject.SetActive(false);
        currentScene = SceneManager.GetSceneAt(1);

        while (canvas.alpha > 0f)
        {
            canvas.alpha -= Time.deltaTime;
            yield return null;
        }

        canvas.gameObject.SetActive(false);
        StopAllCoroutines();
        
    }
}
