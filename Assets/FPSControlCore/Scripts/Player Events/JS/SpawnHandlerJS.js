#pragma strict
import FPSControl;

function OnEnable ()
{
    FPSControlPlayerEvents.OnSpawn += OnSpawn;
}

function OnSpawn()
{
    Debug.Log("Player spawned.");
    //your code here.
}

function OnDisable ()
{
    FPSControlPlayerEvents.OnSpawn += OnSpawn;
}