using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _viewsParent;

    public string Url => _settings.Url.Replace(" ", "%20");

    private static GuiManager _instance;
    public static GuiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GuiManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private Dictionary<Type, BaseView> _views = new Dictionary<Type, BaseView>();
    private BaseView _currentView;
    private SettingsObject _settings;

    private void Awake()
    {
        _settings = Resources.Load<SettingsObject>("Settings");

        foreach (var viewPrefab in _settings.Views)
        {
            var view = Instantiate(viewPrefab, _viewsParent.transform);
            view.gameObject.SetActive(false);
            _views.Add(view.GetType(), view);
        }

        ShowView<MainView>().Init(new MainViewModel());
    }

    public T ShowView<T>() where T : BaseView
    {
        var view = GetViewByType(typeof(T));
        if (view == null)
        {
            Debug.LogError($"View of type {typeof(T)} not founded");
            return null;
        }

        if (_currentView != null)
        {
            _currentView.gameObject.SetActive(false);
        }
        _currentView = view;
        _currentView.gameObject.SetActive(true);

        Screen.fullScreenMode = _currentView.ScreenMode;
        Screen.orientation = _currentView.ScreenOrientation;

        return _currentView as T;
    }

    private BaseView GetViewByType(Type type)
    {
        if (_views.TryGetValue(type, out var view))
        {
            return view;
        }

        return null;
    }
}
