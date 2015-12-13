using UnityEngine;
using System.Collections;
using FPSControl;
public class FreezeHandler : MonoBehaviour
{
    void OnEnable()
    {
        FPSControlPlayerEvents.OnFreeze += OnFreeze;
    }

    void OnFreeze(bool b)
    {
        Debug.Log("Freeze? " + b);
        //your code here.
    }

    void OnDisable()
    {
        FPSControlPlayerEvents.OnFreeze -= OnFreeze;
    }
}
