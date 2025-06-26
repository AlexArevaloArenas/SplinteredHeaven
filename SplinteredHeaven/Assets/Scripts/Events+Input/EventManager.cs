using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class EventManager : MonoBehaviour
{
    //Singleton pattern
    public static EventManager Instance { get; private set; }

    //List of events
    //Game Events
    public event Action ContinueGameEvent;
    public event Action BackToMainMenuEvent;
    public event Action<SceneEnum> ChangeSceneEvent;
    public event Action LoadCurrentMissionSceneEvent;

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

    public event Action<float> MouseWheel;

    public event Action EscapeKeyEvent;

    //public event Action<float> mouseWheel;

    //First person events
    public event Action<TextAsset,Vector3> FPDialogueEvent;
    public event Action EndFPDialogueEvent;
    public event Action<bool,string> FPChangeInteractionSymbolEvent;
    public event Action FixPlayerMovementEvent;
    public event Action RestrictPlayerMovementEvent;
    public event Action FreePlayerMovementEvent;

    //NARRATIVE EVENTS
    public event Action<string> DialogueTriggerEvent;
    public event Action<Vector3> TeleportPlayerEvent;

    public event Action OpenBuildMecha;
    public event Action CloseBuildMecha;


    //BATTLE EVENTS
    public event Action UnitMovesEvent;
    public event Action<MissionInstance> MissionStartsEvent;
    public event Action<MissionInstance> MissionEndsEvent;


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

    public void StartChangeSceneEvent(SceneEnum scene)
    {
        ChangeSceneEvent?.Invoke(scene);
    }

    //HUB EVENTS PLAYER
    public void FixPlayerMovement()
    {
        FixPlayerMovementEvent?.Invoke(); // If Fix Player Movement Event is not null, start it
    }
    public void RectrictPlayerMovement()
    {
        RestrictPlayerMovementEvent?.Invoke();
    }

    public void FreePlayerMovement()
    {
        FreePlayerMovementEvent?.Invoke();
    }

    //Input Events
    public void StartMouseWheelEvent(float _input)
    {
        MouseWheel?.Invoke(_input);
    }
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

    public void StartEscapeKeyEvent()
    {
        EscapeKeyEvent?.Invoke(); // If Escape Key Event is not null, start it
    }

        //Dialogue Events
    public void StartFirstPersonDialogue(TextAsset json, Vector3 pos)
    {
        FPDialogueEvent?.Invoke(json, pos);
    }

    public void ExitFirstPersonDialogue()
    {
        EndFPDialogueEvent?.Invoke();
    }

    public void StartChangeInteractionSymbol(bool show,string text)
    {
        FPChangeInteractionSymbolEvent?.Invoke(show, text);
    }

    //Narrative Events
    public void StartDialogueTriggerEvent(string id)
    {
        DialogueTriggerEvent?.Invoke(id); // If Dialogue Trigger Event is not null, start it
    }

    public void StartTeleportPlayerEvent(Vector3 target)
    {
        TeleportPlayerEvent?.Invoke(target); // If Teleport Player Event is not null, start it
    }

    public void StartOpenBuildMecha()
    {
        OpenBuildMecha?.Invoke(); // If Teleport Player Event is not null, start it
    }
    public void StartCloseBuildMecha()
    {
        CloseBuildMecha?.Invoke(); // If Teleport Player Event is not null, start it
    }


    //Battle Events
    public void StartUnitMoves()
    {
        UnitMovesEvent?.Invoke(); // If Unit Moves Event is not null, start it
    }
    public void StartMission(MissionInstance mission)
    {
        MissionStartsEvent?.Invoke(mission); // If Unit Moves Event is not null, start it
    }
    public void EndMission(MissionInstance mission)
    {
        MissionEndsEvent?.Invoke(mission); // If Unit Moves Event is not null, start it
    }



    //GENERAL EVENTS

    public void StartEventByString(string id)
    {
        var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            Debug.Log($"Checking field: {field.Name} against id: {id}");
            if (string.Equals(field.Name, id, StringComparison.Ordinal))
            {
                if (field.GetValue(this) is Action action)
                {
                    action.Invoke();
                    return;
                }
                else
                {
                    throw new InvalidOperationException($"Field '{id}' is not of type Action.");
                }
            }
        }
    }

}
