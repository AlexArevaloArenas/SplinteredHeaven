using UnityEngine;
using System;
using UnityEngine.Windows;

public class EventManager : MonoBehaviour
{
    //Singleton pattern
    public static EventManager Instance { get; private set; }

    //List of events
    //Game Events
    public event Action ContinueGameEvent;
    public event Action<string> ChangeSceneEvent;

    //Input Events
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action<Vector2> LookEvent;

    public event Action LeftMouseDownEvent;
    public event Action LeftMouseUpEvent;
    public event Action LeftMouseHeldEvent;

    public event Action RightMouseHeldEvent;
    public event Action RightMouseDownEvent;
    public event Action RightMouseUpEvent;

    //First person events
    public event Action<TextAsset> FPDialogueEvent;
    public event Action EndFPDialogueEvent;
    public event Action<bool,string> FPChangeInteractionSymbolEvent;

    //Unit Orders Events
    //public event Action OpenRadialMenu;
    public event Action MoveOrderEvent;
    public event Action<GameObject> AttackOrderEvent;
    public event Action ModuleOrderEvent;

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
    //Game Events
    public void StartContinueGameEvent()
    {
        ContinueGameEvent?.Invoke();
    }

    public void StartChangeSceneEvent(string scene)
    {
        ChangeSceneEvent?.Invoke(scene);
    }

    //Input Events
    public void StartMoveEvent(Vector2 _input)
    {
        MoveEvent?.Invoke(_input);
    }
    public void StartJumpEvent()
    {
        JumpEvent?.Invoke(); // If Jump Event is not null, start it
    }

    public void StartLeftMouseDownEvent()
    {
        LeftMouseDownEvent?.Invoke(); // If Jump Event is not null, start it
    }
    public void StartLeftMouseUpEvent()
    {
        LeftMouseUpEvent?.Invoke(); // If Jump Event is not null, start it
    }
    public void StartLeftMouseHeldEvent()
    {
        LeftMouseHeldEvent?.Invoke(); // If Jump Event is not null, start it
    }

    public void StartRightMouseHeldEvent()
    {
        RightMouseHeldEvent?.Invoke(); // If Jump Event is not null, start it
    }
    public void StartRightMouseUpEvent()
    {
        RightMouseUpEvent?.Invoke(); // If Jump Event is not null, start it
    }
    public void StartRightMouseDownEvent()
    {
        RightMouseDownEvent?.Invoke(); // If Jump Event is not null, start it
    }


    public void StartLookEvent(Vector2 _input)
    {
        LookEvent?.Invoke(_input); // If Jump Event is not null, start it
    }

        //Dialogue Events
    public void StartFirstPersonDialogue(TextAsset json)
    {
        FPDialogueEvent?.Invoke(json);
    }

    public void ExitFirstPersonDialogue()
    {
        EndFPDialogueEvent?.Invoke();
    }

    public void StartChangeInteractionSymbol(bool show,string text)
    {
        FPChangeInteractionSymbolEvent?.Invoke(show, text);
    }

    

}
