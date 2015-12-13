using UnityEngine;
using System.Collections;
using FPSControl;
public class WeaponDeactivateHandler : MonoBehaviour {

    void OnEnable()
    {
        FPSControlPlayerEvents.OnWeaponDeactivate += OnWeaponDeactivate;
    }

    void OnWeaponDeactivate(FPSControlWeapon weapon)
    {
        Debug.Log(weapon.name + " deactivated.");
        //your code here.
    }

    void OnDisable()
    {
        FPSControlPlayerEvents.OnWeaponDeactivate -= OnWeaponDeactivate;
    }
}
