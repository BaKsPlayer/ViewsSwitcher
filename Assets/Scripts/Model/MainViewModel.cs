public class MainViewModel : IMainViewModel
{
    public void OpenWebView()
    {
        var view = GuiManager.Instance.ShowView<WebView>();
        view.Init(new WebViewModel());
    }
}
