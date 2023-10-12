using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{

    public GameObject machineChoiceCanvas;

    public List<Button> machineChoice;
    public List<GameObject> machinesPrefab;

    private BlueprintMode _blueprintMode;
    public GameObject machineStock;
    public GameObject machineSelectedPlacementMode;
    
    private void Awake()
    {
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
    }

    void OnEnable()
    {
        machineChoiceCanvas.SetActive(true);
    }

    void OnDisable()
    {
        machineChoiceCanvas.SetActive(false);
    }

    private void Start()
    {
        for (var i = 0; i < machineChoice.Count; i++)
        {
            var a = i;
            machineChoice[i].onClick.AddListener(() => { MachineChosen(a); });
        }
    }

    void MachineChosen(int machineNumber)
    {
        //instantiate un holograme puis avec le mode deplacement, le machine suis la souris avec un raycast qui ne touche que le Layer Baseground
        machineSelectedPlacementMode = Instantiate(machinesPrefab[machineNumber], machineStock.transform);
        _blueprintMode.nextModesRequirements[0] = true;
    }
}
