#pragma downcast
import FPSControl;

	var weaponName : String = "Beretta";
	var gui : GUIText;
	function Awake()
	{
		gui = GetComponent(GUIText);
	}
	function Update()
	{
		//Debug.Log( "Current Weapon: " + FPSControlPlayerData.currentWeapon );
		
		//If the current weapon is the Beretta
		if(FPSControlPlayerData.currentWeapon == FPSControlPlayerData.GetWeapon(weaponName))
		{
			var b = FPSControlPlayerData.GetWeapon(weaponName) as FPSControlRangedWeapon;
			
			//Get the Ammo
			var ammo = new Array();
			
			//In an int array: _currentClipContents, _currentClips, _currentAmmo
			ammo = FPSControlPlayerData.GetAmmo(b);
			
			
			
			//If we still have ammo clips left, but the current clip is empty
			if(ammo[2] > 0 && ammo[0] == 0)
			{
				//display the 'reload' message
				gui.text = "Use reload key to reload";
			}
			
			else if(ammo[0] == 0 && ammo[1] == 0 && ammo[2] == 0)
			{
				gui.text = "Out of ammo.";
			}
			
			else
			
			{
				gui.text = "Clip Contents: "+ammo[0]+" Clips:"+ammo[1]+" Ammo:"+ammo[2];
			}
			
			if(Input.GetKeyDown (KeyCode.F1))
			{
				b.AddAmmo(1);
			}
		}
		else
		{
			gui.text = "";
		}
	}
	