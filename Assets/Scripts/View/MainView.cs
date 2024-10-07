using UnityEngine;

public class MainView : BaseView
{ 
    private IMainViewModel _viewModel;

    public override FullScreenMode ScreenMode => FullScreenMode.ExclusiveFullScreen;
    public override ScreenOrientation ScreenOrientation => ScreenOrientation.Portrait;

    public void Init(IMainViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void OpenWebView()
    {
        _viewModel.OpenWebView();
    }
}
