using UnityEngine;

public class ShortcutBarInteractions : MonoBehaviour
{
    public GameObject shortcutBar;
    private void OnEnable()
    {
        shortcutBar.SetActive(true);
    }

    private void OnDisable()
    {
        shortcutBar.SetActive(false);
    }
}
