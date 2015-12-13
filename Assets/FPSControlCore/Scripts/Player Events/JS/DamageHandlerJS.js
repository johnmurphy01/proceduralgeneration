#pragma strict
import FPSControl;

function OnEnable ()
{
    FPSControlPlayerEvents.OnReceiveDamage += OnReceiveDamage;
}

function OnReceiveDamage(src : DamageSource)
{
    Debug.Log("Received Damage from " + src);
    //your code here.
}

function OnDisable ()
{
    FPSControlPlayerEvents.OnReceiveDamage += OnReceiveDamage;
}