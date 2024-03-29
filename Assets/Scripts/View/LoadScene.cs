using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas;
    [SerializeField]
    private Camera camera;
    private Scene currentScene;

    private void Start()
    {
        //LoadThisScene(1);
        ButtonLoadScene.OnLoadButtonClicked += LoadThisScene;
        ButtonLoadLevel.OnLoadLevelButtonClicked += LoadThisLevel;
        ButtonLoadAlienCosmetic.OnLoadAlienCosmeticButtonClicked += LoadThisAlienCosmetic;
    }

    private void OnDisable()
    {
        ButtonLoadScene.OnLoadButtonClicked -= LoadThisScene;
        ButtonLoadLevel.OnLoadLevelButtonClicked -= LoadThisLevel;
        ButtonLoadAlienCosmetic.OnLoadAlienCosmeticButtonClicked -= LoadThisAlienCosmetic;
    }


    public void LoadThisScene(int sceneIndex)
    {
        Debug.Log("Pulsado");
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    public void LoadThisLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelToLoad", levelId);
        LoadThisScene(2);
    }

    public void LoadThisAlienCosmetic(int alienId)
    {
        PlayerPrefs.SetInt("AlienToLoad", alienId);
        LoadThisScene(6);
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

        Debug.Log("Antes de toda carga");

        while (canvas.alpha < 1f)
        {
            canvas.alpha += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Despues alpha");

        foreach (Camera c in Camera.allCameras)
        { if (c != camera) { c.gameObject.SetActive(false); } }
        camera.gameObject.SetActive(true);

        if (currentScene.isLoaded) {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
            while (!unloadOperation.isDone)
            { yield return null; }
            Debug.Log("Despues Unload");
        }
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            { yield return null; }
            currentScene = SceneManager.GetSceneAt(0);
            Debug.Log("Despues carga");
            /*while (SceneManager.GetActiveScene().buildIndex != sceneIndex)
            {
                yield return null;
            }*/

            Debug.Log("Dewspues carga 2");
        }

        while (SceneManager.sceneCount > 2)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(2));
        }

        Debug.Log("Eliminar scenes");

        camera.gameObject.SetActive(false);
        

        /*while (canvas.alpha > 0f)
        {
            canvas.alpha -= Time.deltaTime;
            yield return null;
        }*/

        

        Debug.Log("fin");

        canvas.gameObject.SetActive(false);
        currentScene = SceneManager.GetSceneAt(1);
        StopAllCoroutines();
        
    }
}
