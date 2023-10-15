using UnityEngine;

public class OnOffPlayerModeSelect : MonoBehaviour
{
    public GameObject playerModeSelect;
    private PlayerModeSelect _playerModeSelect;
    
    private void Awake()
    {
        _playerModeSelect = playerModeSelect.GetComponent<PlayerModeSelect>();
    }

    private void OnEnable()
    {
        _playerModeSelect.canvasGroupMode.alpha = 0;
        playerModeSelect.SetActive(false);
    }
}