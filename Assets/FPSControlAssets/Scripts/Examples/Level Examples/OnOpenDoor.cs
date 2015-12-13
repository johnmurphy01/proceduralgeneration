using UnityEngine;
using System.Collections;
using FPSControl;

public class OnOpenDoor : InteractiveObject
{

    public override void Interact()
    {
        //Debug.Log("I AM THE ONE WHO KNOCKS");
        FPSControlPlayerData.SaveTempWeaponData(); // Save the temp data for changing scenes
        FPSControlPlayerData.SaveWeaponData(GameSaveExample.Slot); // Save for new session.

        GameSaveExample.currentSaveData.currentHealth = FPSControlPlayerData.healthData.current;

        Application.LoadLevel("SaveTestB");
    }
}
