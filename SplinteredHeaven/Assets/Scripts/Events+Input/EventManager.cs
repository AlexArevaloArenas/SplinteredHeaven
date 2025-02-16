using UnityEngine;
using System;
using UnityEngine.Windows;

public class EventManager : MonoBehaviour
{
    //Singleton pattern
    public static EventManager Instance { get; private set; }

    //List of events
        //Input Events
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action LeftMouseEvent;
    public event Action<Vector2> LookEvent;

    //First person events
    public event Action FPDialogueEvent;
    public event Action<bool,string> FPChangeInteractionSymbolEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { 
            Destroy(Instance);
        }
        //DontDestroyOnLoad(gameObject);
    }

    //List of event callers
        //Input Events
    public void StartMoveEvent(Vector2 _input)
    {
        MoveEvent?.Invoke(_input);
    }
    public void StartJumpEvent()
    {
        JumpEvent?.Invoke(); // If Jump Event is not null, start it
    }

    public void StartLeftMouseEvent()
    {
        LeftMouseEvent?.Invoke(); // If Jump Event is not null, start it
    }

    public void StartLookEvent(Vector2 _input)
    {
        LookEvent?.Invoke(_input); // If Jump Event is not null, start it
    }

        //Dialogue Events
    public void StartFirstPersonDialogue()
    {
        FPDialogueEvent?.Invoke();
    }

    public void StartChangeInteractionSymbol(bool show,string text)
    {
        FPChangeInteractionSymbolEvent?.Invoke(show, text);
    }

    

}
