using UnityEngine;
using UnityEngine.UI;

public class MinimapSymbol : MonoBehaviour
{
    public UnitManager unit;
    public SpriteRenderer img;
    public Color enemy;
    public Color ally;

    private void Start()
    {
        SpriteRenderer img = GetComponent<SpriteRenderer>();
        if (unit.gameObject.tag == "Enemy")
        {
            img.color = enemy;
        }
        else
        {
            img.color = ally;
        }
    }

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
