using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Objet/Recipe", order = 2)]
public class Recipe : ScriptableObject
{
    public List<ItemClass> inputs;
    public List<int> inputsAmount;

    public List<ItemClass> outputs;
    public List<int> outputsAmount;
}
