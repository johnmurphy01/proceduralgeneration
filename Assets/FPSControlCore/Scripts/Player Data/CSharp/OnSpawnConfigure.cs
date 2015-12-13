using UnityEngine;
using System.Collections;
using FPSControl;

public class OnSpawnConfigure : MonoBehaviour {

    public FPSControlRangedWeapon beretta;

	// Use this for initialization
	void Awake () {
        FPSControlPlayerEvents.OnSpawn += OnSpawn;
	}

    void OnSpawn()
    {
        FPSControlPlayerData.frozen = false;
        FPSControlPlayerData.visible = true;

        //FPSControlPlayerData.AddWeapon("Beretta",true);
        beretta = (FPSControlRangedWeapon)FPSControlPlayerData.GetWeapon("Beretta");
        beretta.SetAmmo(10, 2);

		FPSControlPlayerData.healthData.current = FPSControlPlayerData.healthData.max;
    }
}
