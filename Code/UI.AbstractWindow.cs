using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.UI;

public class AbstractWindow<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public static Transform content_transform;
    public static Transform background_transform;
    internal static bool initialized = false;
    internal static bool is_open = false;
    internal static bool first_open = true;

    public static void base_init(string window_id)
    {
        ScrollWindow scroll_window = Windows.CreateNewWindow(window_id, window_id + Constants.Core.title_suffix);
        GameObject window_object = scroll_window.gameObject;
        instance = window_object.AddComponent<T>();
        instance.gameObject.SetActive(false);

        background_transform = scroll_window.transform.Find("Background");
        background_transform.Find("Scroll View").gameObject.SetActive(true);

        LocalizedText localized_text = background_transform.Find("Title").gameObject.AddComponent<LocalizedText>();
        localized_text.text = background_transform.Find("Title").GetComponent<Text>();
        localized_text.key = window_id + Constants.Core.title_suffix;

        content_transform = background_transform.Find("Scroll View/Viewport/Content");
    }
}