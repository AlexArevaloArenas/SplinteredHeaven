using System.Collections;
using UnityEngine;

public class Autodestruct : MonoBehaviour
{
    public float lifetime = 1.0f; // Lifetime of the object in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroy()); // Start the coroutine to destroy the object after its lifetime
    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject); // Destroy the game object after the specified lifetime
    }
}
