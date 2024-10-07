using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public abstract FullScreenMode ScreenMode { get; }
    public abstract ScreenOrientation ScreenOrientation { get; }
}
