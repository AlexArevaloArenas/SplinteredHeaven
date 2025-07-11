using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference characterTalk { get; private set; }

    [field: Header("Menu SFX")]
    [field: SerializeField] public EventReference acceptSFX { get; private set; }
    [field: SerializeField] public EventReference denySFX { get; private set; }
    [field: SerializeField] public EventReference step { get; private set; }

    [field: Header("Battle SFX")]
    [field: SerializeField] public EventReference messageSFX { get; private set; }
    [field: SerializeField] public EventReference notifySFX { get; private set; }
    [field: SerializeField] public EventReference shoot { get; private set; }
    [field: SerializeField] public EventReference explosion { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }

}