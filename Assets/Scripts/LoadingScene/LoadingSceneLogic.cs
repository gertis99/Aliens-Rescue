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

    void Awake()
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

        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        //create services
        GameConfigService gameConfig = new GameConfigService();
        GameProgressionService gameProgression = new GameProgressionService();

        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4928651", "Rewarded_Android");
        UnityIAPGameService iapService = new UnityIAPGameService();
        IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();

        //register services
        //Debug.Log("Z");
        ServiceLocator.RegisterService(gameConfig);
        //Debug.Log("Z");
        ServiceLocator.RegisterService(gameProgression);
        //Debug.Log("Z");
        ServiceLocator.RegisterService(remoteConfig);
        //Debug.Log("Z");
        ServiceLocator.RegisterService(loginService);
        //Debug.Log("Z");
        ServiceLocator.RegisterService(adsService);
        //Debug.Log("Z");
        ServiceLocator.RegisterService(analyticsService);
        //Debug.Log("Z");
        ServiceLocator.RegisterService<IIAPGameService>(iapService);

        //initialize services
        Debug.Log("A");
        await servicesInitializer.Initialize();
        Debug.Log("B");
        await loginService.Initialize();
        Debug.Log("C");
        await remoteConfig.Initialize();
        Debug.Log("D");
        await analyticsService.Initialize();
        Debug.Log("E");
        await iapService.Initialize(new Dictionary<string, string>
        {
            ["test1"] = "es.gmangames.alienrescue.test1"
        });

        Debug.Log("F");
        await gameProgressionProvider.Initialize();
        Debug.Log("G");
        gameConfig.Initialize(remoteConfig);
        Debug.Log("H");
        gameProgression.Initialize(gameConfig, gameProgressionProvider);

        Debug.Log("I");
        bool adsInitialized = await adsService.Initialize(Application.isEditor);
            
        Debug.Log("AdsInitialized: " + adsInitialized);

        loadScene.LoadThisScene(1);
    }
}