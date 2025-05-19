using UnityEngine;

public abstract class SceneUI<TManager, TUI> : MonoBehaviour, ISceneUI
    where TManager : UIManager<TUI>
    where TUI : class
{
    protected virtual void Start()
    {
        var manager = GetManagerInstance();
        if (manager != null)
        {
            manager.RegisterUI(this as TUI);
        }
        InitializeUI();
    }
        
    protected abstract TManager GetManagerInstance();

    public virtual void InitializeUI() { }
}