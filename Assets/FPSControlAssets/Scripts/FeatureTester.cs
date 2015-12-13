using UnityEngine;
using System.Collections;
using FPSControl;

public class FeatureTester : MonoBehaviour
{

	public GameObject		_player;
	public GameObject		_hurtIndicator;
	public MusicControl		_musicControl;
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetKeyUp( KeyCode.Minus ) )
		{
			_player.SendMessage("ApplyDamage", 5, SendMessageOptions.DontRequireReceiver);
			Debug.Log( "player damage" );
		}
		
		if( Input.GetKeyUp( KeyCode.Alpha7 ) ) HurtIndicator.GotHurtQuadrant( 5.0F, HurtQuadrant.FRONT );
		if( Input.GetKeyUp( KeyCode.Alpha8 ) ) HurtIndicator.GotHurtQuadrant( 5.0F, HurtQuadrant.BACK );
		if( Input.GetKeyUp( KeyCode.Alpha9 ) ) HurtIndicator.GotHurtQuadrant( 5.0F, HurtQuadrant.LEFT );
		if( Input.GetKeyUp( KeyCode.Alpha0 ) ) HurtIndicator.GotHurtQuadrant( 5.0F, HurtQuadrant.RIGHT );
		
		if( Input.GetKeyUp( KeyCode.Equals ) )
		{
			Debug.Log( "mm " + _musicControl.selectedTrack );
			//MessengerControl.Broadcast("StopTrack",true);
			MessengerControl.Broadcast( "FadeIn", "Tension" );
		}
	}
}
