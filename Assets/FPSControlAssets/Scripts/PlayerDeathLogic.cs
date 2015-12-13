using UnityEngine;
using System.Collections;
using FPSControl;

public class PlayerDeathLogic : MonoBehaviour {

	private FPSControlPlayer player;

	// Use this for initialization
	void Start () {
		player = GetComponent<FPSControlPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayerDied()
	{
		player.Respawn();
	}
}
