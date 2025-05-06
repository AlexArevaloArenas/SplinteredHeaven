using UnityEngine;

public class TerrainMark : MonoBehaviour
{
    private float Timer = 0;
    private float Scale = 200;
    public float _heightDistance = 0.0001f;
    private void Update()
    {
        Timer += Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x + Mathf.Sin(Timer * 10) / Scale, transform.localScale.y, transform.localScale.z + Mathf.Sin(Timer * 10) / Scale);
        if (Timer > 1.2)
        {
            Timer = 0;
            gameObject.SetActive(false);
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }
}
