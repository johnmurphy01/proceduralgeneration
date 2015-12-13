#pragma strict
import FPSControl;

function OnEnable ()
{
    FPSControlPlayerEvents.OnFreeze += OnFreeze;
}

function OnFreeze(b : boolean)
{
    Debug.Log("Freeze? " + b);
    //your code here.
}

function OnDisable ()
{
    FPSControlPlayerEvents.OnFreeze += OnFreeze;
}