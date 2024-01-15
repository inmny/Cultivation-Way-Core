using UnityEngine;

namespace Cultivation_Way.UI;

public abstract class AbstractWindowAddition : MonoBehaviour
{
    protected RectTransform Background;
    protected RectTransform Content;
    protected bool initialized;

    public void Init()
    {
        if (initialized) return;
        Background = transform.Find("Background") as RectTransform;
        Content = Background.Find("Scroll View/Viewport/Content") as RectTransform;
        Initialize();
        initialized = true;
    }

    protected abstract void Initialize();
}