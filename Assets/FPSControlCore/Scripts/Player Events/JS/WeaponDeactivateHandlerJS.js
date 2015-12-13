#pragma strict
import FPSControl;

function OnEnable ()
{
    FPSControlPlayerEvents.OnWeaponDeactivate += OnWeaponDeactivate;
}

function OnWeaponDeactivate(weapon : FPSControlWeapon)
{
    Debug.Log(weapon.name + " deactivated.");
    //your code here.
}

function OnDisable ()
{
    FPSControlPlayerEvents.OnWeaponDeactivate += OnWeaponDeactivate;
}