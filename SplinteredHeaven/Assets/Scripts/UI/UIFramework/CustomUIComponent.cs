using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public abstract class CustomUIComponent : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }
    public abstract void Setup();
    public abstract void Configure();

    //[Button("Configure Now")]
    private void Init()
    {
        Setup();
        Configure();
    }

    private void OnValidate()
    {
        Init();
    }
}
