using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneLogic : MonoBehaviour
{
    [SerializeField]
    private bool IsDevBuild = true;
    public LoadScene loadScene;

    private TaskCompletionSource<bool> _cancellationTaskSource;

    void Start()
    {
        Debug.Log("Antes de todo");
        _cancellationTaskSource = new();
        LoadServicesCancellable().ContinueWith(task =>
                Debug.LogException(task.Exception),
            TaskContinuationOptions.OnlyOnFaulted);
        Debug.Log("Z");
    }

    private void OnDestroy()
    {
        _cancellationTaskSource.SetResult(true);
    }

    private async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    private async Task LoadServices()
    {
        string environmentId = IsDevBuild ? "development" : "production";

        Debug.Log("B");
        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        Debug.Log("C");
        //create services
        GameConfigService gameConfig = new GameConfigService();
        Debug.Log("D");
        GameProgressionService gameProgression = new GameProgressionService();
        Debug.Log("E");

        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        Debug.Log("F");
        LoginGameService loginService = new LoginGameService();
        Debug.Log("G");
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        Debug.Log("H");
        AdsGameService adsService = new AdsGameService("4928651", "Rewarded_Android");
        Debug.Log("I");
        UnityIAPGameService iapService = new UnityIAPGameService();
        Debug.Log("J");

        //register services
        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService<IIAPGameService>(iapService);
        Debug.Log("K");

        //initialize services
        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        await iapService.Initialize(new Dictionary<string, string>
        {
            ["test1"] = "es.gmangames.alienrescue.test1"
        });
        Debug.Log("L");

        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(gameConfig);
        Debug.Log("M");

        bool adsInitialized = await adsService.Initialize(Application.isEditor);
            
        Debug.Log("AdsInitialized: " + adsInitialized);

        loadScene.LoadThisScene(1);
    }
}