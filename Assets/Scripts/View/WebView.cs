using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebView : BaseView
{
    [SerializeField]
    private WebViewObject _webViewObject;

    public override FullScreenMode ScreenMode => FullScreenMode.Windowed;
    public override ScreenOrientation ScreenOrientation => ScreenOrientation.AutoRotation;

    private IWebViewModel _viewModel;
    private ScreenOrientation _lastOrientation;

    public void Init(IWebViewModel viewModel)
    {
        _viewModel = viewModel;
        StartCoroutine(InitInternal());
    }

    private void Start()
    {
        _lastOrientation = Screen.orientation;
    }

    private void Update()
    {
        if (Screen.orientation != _lastOrientation)
        {
            _lastOrientation = Screen.orientation;
            OnOrientationChanged();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    private void HandleBackButton()
    {
        if (_webViewObject.CanGoBack())
        {
            _webViewObject.GoBack();
        }
    }

    private void OnOrientationChanged()
    {
        SetMargins();
    }

    private IEnumerator InitInternal()
    {
        _webViewObject.Init();

        while (!_webViewObject.IsInitialized())
        {
            yield return null;
        }

        SetMargins();
        _webViewObject.SetVisibility(true);

        #region LoadUrl
#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        if (_viewModel.Url.StartsWith("http"))
        {
            _webViewObject.LoadURL(_viewModel.Url);
        }
        else
        {
            var exts = new string[]{
                ".jpg",
                ".js",
                ".html"  // should be last
            };
            foreach (var ext in exts)
            {
                var url = _viewModel.Url.Replace(".html", ext);
                var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
                var dst = System.IO.Path.Combine(Application.temporaryCachePath, url);
                byte[] result = null;
                if (src.Contains("://"))
                { 
#if UNITY_2018_4_OR_NEWER
                    var unityWebRequest = UnityWebRequest.Get(src);
                    yield return unityWebRequest.SendWebRequest();
                    result = unityWebRequest.downloadHandler.data;
#else
                    var www = new WWW(src);
                    yield return www;
                    result = www.bytes;
#endif
                }
                else
                {
                    result = System.IO.File.ReadAllBytes(src);
                }
                System.IO.File.WriteAllBytes(dst, result);
                if (ext == ".html")
                {
                    _webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
                    break;
                }
            }
        }
#else
        if (_viewModel.Url.StartsWith("http"))
        {
            _webViewObject.LoadURL(_viewModel.Url);
        } else
        {
            _webViewObject.LoadURL("StreamingAssets/" + _viewModel.Url);
        }
#endif
        yield break;
        #endregion
    }

    private void SetMargins()
    {
        _webViewObject.SetMargins(0, 0, 0, 0);
    }
}
