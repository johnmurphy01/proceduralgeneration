using UnityEngine;
using System.Collections;
using FPSControl;

public class DeathHandler : MonoBehaviour {

    void OnEnable()
    {
        FPSControlPlayerEvents.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        Debug.Log("Player died.");
        //your code here.
    }

    void OnDisable()
    {
        FPSControlPlayerEvents.OnDeath -= OnDeath;
    }
}
