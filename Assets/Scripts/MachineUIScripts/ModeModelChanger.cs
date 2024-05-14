using UnityEngine;

public class ModeModelChanger : MonoBehaviour
{
    
    public Animator objectModel;

    public int HMBswitch;
    
    private void OnEnable()
    {
        for(var i = 0; i < objectModel.parameters.Length; i++)
        {
            objectModel.SetBool(objectModel.GetParameter(i).name, false);
        }
        
        objectModel.SetBool(objectModel.GetParameter(HMBswitch).name, true);
    }
}
