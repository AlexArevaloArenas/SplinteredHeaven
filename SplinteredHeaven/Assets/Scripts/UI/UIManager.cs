using UnityEngine;

public abstract class UIManager<TUI> : MonoBehaviour where TUI : class
{
    /*
    public static UIManager<TUI> Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    */

    protected TUI currentUI;

    public virtual void RegisterUI(TUI newUI)
    {
        currentUI = newUI;
    }

    public virtual void ClearUI()
    {
        currentUI = null;
    }

    void OnDisable()
    {
        ClearUI();

    }
}