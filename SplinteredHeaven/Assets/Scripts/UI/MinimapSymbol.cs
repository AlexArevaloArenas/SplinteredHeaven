using UnityEngine;

public class MinimapSymbol : MonoBehaviour
{
    public UnitManager unit;

    private void Update()
    {
        if (unit.visible)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
