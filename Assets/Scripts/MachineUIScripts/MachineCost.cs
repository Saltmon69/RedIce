using System.Collections.Generic;
using UnityEngine;

public class MachineCost : MonoBehaviour
{
    [SerializeField] public List<ItemClass> buildingMaterialList;
    [SerializeField] public List<int> buildingMaterialAmountList;
    [SerializeField] public int machinePowerCost;
}
