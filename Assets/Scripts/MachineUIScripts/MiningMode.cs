using UnityEngine;

public class MiningMode : MonoBehaviour
{
    private PlayerInteraction _playerInteraction;

    public void Awake()
    {
        _playerInteraction = GameObject.FindWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public void OnEnable()
    {
        _playerInteraction.isMiningModeActive = true;
    }

    public void OnDisable()
    {
        _playerInteraction.isMiningModeActive = false;
    }
}
