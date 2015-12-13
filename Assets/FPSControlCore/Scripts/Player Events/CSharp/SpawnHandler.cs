using UnityEngine;
using System.Collections;
using FPSControl;
public class SpawnHandler : MonoBehaviour
{
    void OnEnable()
    {
        FPSControlPlayerEvents.OnSpawn += OnSpawn;
    }

    void OnSpawn()
    {
        Debug.Log("Player spawned.");
        //your code here.
    }

    void OnDisable()
    {
        FPSControlPlayerEvents.OnSpawn -= OnSpawn;
    }
}
