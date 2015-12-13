using UnityEngine;
using System.Collections;
using FPSControl;

public class DamageHandler : MonoBehaviour
{
    void OnEnable()
    {
        FPSControlPlayerEvents.OnReceiveDamage += OnReceiveDamage;
    }

    void OnReceiveDamage(DamageSource src)
    {
        Debug.Log("Received Damage from " + src);
        //your code here.
    }

    void OnDisable()
    {
        FPSControlPlayerEvents.OnReceiveDamage -= OnReceiveDamage;
    }
}

