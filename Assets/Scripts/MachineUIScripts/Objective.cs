using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class Objective : MonoBehaviour
{
    public GameObject thisGameObject;

    public List<MonoBehaviour> mono;
    
    [Space(10)]
    public int scriptReference;

    public MonoBehaviour thisScript;

    public List<FieldInfo> fieldList;

    public List<string> fieldNameList;

    [Space(10)]
    public int fieldReference;
    public string fieldReferenceName;

    public FieldInfo thisField;

    [Space(10)] 
    [Range(0,2)] public int intFloatBool;
    [Range(0,2)] public int moreLessEqual;

    [Space(10)] 
    public int wantedIntValue;
    public float wantedFloatValue;
    public bool wantedBoolValue;

    [Space(10)] 
    public bool playMode;
    public bool isObjectiveCompleted;
    public bool updateEditor;

    public void Start()
    {
        playMode = true;
        ObjectiveSetup();
    }

    public void Update()
    {
        playMode = Application.isPlaying;
        
        if(!playMode)
        {
            ObjectiveSetup();
        }

        var thisValue = thisField.GetValue(thisScript);
        //Debug.Log(thisValue);

        if(playMode)
        {
            switch(intFloatBool)
            {
                case 0:
                    switch(moreLessEqual)
                    {
                        case 0:
                            if(int.Parse(thisValue.ToString()) > wantedIntValue) isObjectiveCompleted = true;
                            break;
                        case 1:
                            if(int.Parse(thisValue.ToString()) < wantedIntValue) isObjectiveCompleted = true;
                            break;
                        case 2:
                            if(int.Parse(thisValue.ToString()) == wantedIntValue) isObjectiveCompleted = true;
                            break;
                    }
                    break;
                case 1:
                    switch(moreLessEqual)
                    {
                        case 0:
                            if((float)thisValue >= wantedFloatValue) isObjectiveCompleted = true;
                            break;
                        case 1:
                            if((float)thisValue <= wantedFloatValue) isObjectiveCompleted = true;
                            break;
                        case 2:
                            print("this option isn't available");
                            break;
                    }
                    break;
                case 2:
                    if((bool)thisValue == wantedBoolValue) isObjectiveCompleted = true;
                    break;
            }
            
            if(isObjectiveCompleted)
            {
                print("you have completed your objective");
                if (this.gameObject.transform.parent.childCount > 1) this.gameObject.transform.parent.GetChild(1).GetComponent<Objective>().enabled = true;
                Destroy(this.gameObject);
            }
        }
    }

    public void ObjectiveSetup()
    {
        mono = new List<MonoBehaviour>();
        fieldList = new List<FieldInfo>();
        fieldNameList = new List<string>();

        mono.AddRange(thisGameObject.GetComponents<MonoBehaviour>());
        
        thisScript = mono[scriptReference];
        
        fieldList.AddRange(thisScript.GetType().GetFields());

        for(var i = 0; i < fieldList.Count; i++)
        {
            fieldNameList.Add(fieldList[i].Name);
        }

        thisField = fieldList[fieldReference];
        fieldReferenceName = thisField.Name;

        if(thisField == typeof(int)) intFloatBool = 1;
        if(thisField == typeof(float)) intFloatBool = 2;
        if(thisField == typeof(bool)) intFloatBool = 3;
    }
}
