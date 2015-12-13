using UnityEngine;
using System.Collections;
using FPSControl;

public class PlayerDataExample : MonoBehaviour {

    public Vector2 cameraPitchConstraints = new Vector2(-65, 90); //min and max Y, not x-axis/y-axis since x is also turning the player's body
    public bool canRun = true;

    public float runForwardSpeed = 7;
    public float runStrafeSpeed = 5;
    public float runReverseSpeed = 6;

    public float jumpHeight;
    public bool forceCrouching = false;
    public Vector3 gravity;

    public bool frozen = false;
    public bool visible = true;

    public bool autoHeal = false;
    public float healSpeed, minHealth, maxHealth, startHealth;

    void Awake()
    {
        FPSControlPlayerEvents.OnSpawn += OnSpawn;
    }

    void Update()
    {
        //freeze
        if (FPSControlPlayerData.frozen != frozen) FPSControlPlayerData.frozen = frozen; //will dispatch FPSControlPlayerEvents.OnFreeze event on set

        //visible
        if (FPSControlPlayerData.visible != visible) FPSControlPlayerData.visible = visible;

        //gravity
        FPSControlPlayerData.gravity = gravity;
    }

    void OnSpawn()
    {
        /*
         * NOTE: This is simply an example of how to get/set player data, not a literal usage example.
         * 
         */
        //camera
        FPSControlPlayerData.cameraPitchConstraints = cameraPitchConstraints;

        //running
        FPSControlPlayerData.canRun = canRun;
        FPSControlPlayerData.runForwardSpeed = runForwardSpeed;
        FPSControlPlayerData.runReverseSpeed = runReverseSpeed;
        FPSControlPlayerData.runStrafeSpeed = runStrafeSpeed;

        //crouching
        FPSControlPlayerData.forceCrouching = forceCrouching;

        //health
        DataController healthData = FPSControlPlayerData.healthData; //gets the healthdata component
        healthData.autoHeal = autoHeal;
        healthData.healingSpeed = healSpeed;
        healthData.min = minHealth;
        healthData.max = maxHealth;
        healthData.current = healthData.initial = startHealth;

        //weapons
        //FPSControlPlayerData.EquipWeapon("Beretta"); //equips the weapon with the specified name
        FPSControlPlayerData.EquipWeaponAt(1); //equips the second weapon in our array of available weapons.
        //FPSControlPlayerData.DeactivateCurrentWeapon();//deactivates the current weapon, will dispatch FPSControlPlayerEvents.OnWeaponDeactivate event
        
        FPSControlRangedWeapon beretta = (FPSControlRangedWeapon)FPSControlPlayerData.GetWeapon("Beretta"); //gets the weapon named "Beretta"
        if(beretta) FPSControlPlayerData.SetAmmo(beretta, 10, 5); //(energyweapon,.5F) overload for energy weapon
	}

}
