#pragma strict
import FPSControl;

function OnEnable ()
{
    FPSControlPlayerEvents.OnDeath += OnDeath;
}

function OnDeath()
{
    Debug.Log("Player died.");
    //your code here.
}

function OnDisable ()
{
    FPSControlPlayerEvents.OnDeath += OnDeath;
}