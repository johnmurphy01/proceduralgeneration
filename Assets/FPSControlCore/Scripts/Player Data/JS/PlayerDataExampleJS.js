#pragma strict
import FPSControl;

    public var cameraPitchConstraints : Vector2 = new Vector2(-65, 90); //min and max Y, not x-axis/y-axis since x is also turning the player's body
    public var canRun : boolean = true;

    public var runForwardSpeed : float = 7;
    public var runStrafeSpeed : float = 5;
    public var runReverseSpeed : float = 6;

    public var jumpHeight : float;
    public var forceCrouching : boolean = false;
    public var gravity : Vector3;

    public var frozen : boolean = false;
    public var visible : boolean = true;

    public var autoHeal = false;
    public var healSpeed : float;
    public var minHealth : float;
    public var maxHealth : float;
    public var startHealth : float;

    function Awake()
    {
        FPSControlPlayerEvents.OnSpawn += OnSpawn;
    }

    function Update()
    {
        //freeze
        if(FPSControlPlayerData.frozen != frozen) FPSControlPlayerData.frozen = frozen; //will dispatch FPSControlPlayerEvents.OnFreeze event on set

        //visible
        if(FPSControlPlayerData.visible != visible) FPSControlPlayerData.visible = visible;

        //gravity
        FPSControlPlayerData.gravity = gravity;

    }

    function OnSpawn()
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
        var healthData : DataController = FPSControlPlayerData.healthData; //gets the healthdata component
        healthData.autoHeal = autoHeal;
        healthData.healingSpeed = healSpeed;
        healthData.min = minHealth;
        healthData.max = maxHealth;
        healthData.current = healthData.initial = startHealth;

        //weapons
        var beretta : FPSControlRangedWeapon = FPSControlPlayerData.GetWeapon("Beretta") as FPSControlRangedWeapon; //gets the weapon named "Beretta"
        FPSControlPlayerData.EquipWeapon("Beretta"); //equips the weapon with the specified name
        FPSControlPlayerData.EquipWeaponAt(1); //equips the second weapon in our array of available weapons.
        FPSControlPlayerData.DeactivateCurrentWeapon();//deactivates the current weapon, will dispatch FPSControlPlayerEvents.OnWeaponDeactivate event

        FPSControlPlayerData.SetAmmo(beretta, 10, 5); //(energyweapon,.5F) overload for energy weapon
	}