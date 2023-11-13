using UnityEngine;

public class RemovePlayerMode : MonoBehaviour
{
    public GameObject playerModeSelect;
    private void OnEnable()
    {
        playerModeSelect.SetActive(true);
    }
}
