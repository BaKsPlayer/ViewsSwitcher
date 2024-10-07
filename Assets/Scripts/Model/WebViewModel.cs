public class WebViewModel : IWebViewModel
{
    public string Url { get; }

    public WebViewModel()
    {
        Url = GuiManager.Instance.Url;
    }
}
