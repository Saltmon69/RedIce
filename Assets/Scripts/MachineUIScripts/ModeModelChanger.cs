using UnityEngine;

public class ModeModelChanger : MonoBehaviour
{
    public GameObject objectModel;

    private void OnEnable()
    {
        objectModel.SetActive(true);
    }

    private void OnDisable()
    {
        objectModel.SetActive(false);
    }
}
