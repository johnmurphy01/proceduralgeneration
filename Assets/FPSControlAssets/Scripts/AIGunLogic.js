//=====
//=====


enum AIShootDirection
{
	Right,
	Left,
	Forward,
	Backward
}

enum Accuracy
{
	Poor,
	Medium,
	High
}

// weapon stats
var				_weaponsName :			String;
var				_weaponOwner :          GameObject;

//var				_animator :				GameObject;
var     accuracy :	Accuracy = Accuracy.Medium;
var				_damage :				float							= 5f;
var				_ignoreColliders :	Collider[];
var				_layerMask :			LayerMask;
var				_pickupVersion :		GameObject;
var				_usesAmmo :				boolean						= true;
var				_hideWeapon :			boolean						= false;
var				_weapon :				GameObject;
 // make sure you have enough ammo gui pics to cover this amount. pic array length will be divided into this numbers
var				_maxAmmo :				float							= 200;
var				_burstMax :				int							= 0;
var				_burstDelay :			float							= 0.0;
private var		_burstCount :			int							= 0;
private var		_burstTimer :			float							= 0.0;
var				_emptyMagazineSnd :	AudioClip;
var				_reloadingTime :		float;
//--- Weapon related
var				_shootDirection :		AIShootDirection				= AIShootDirection.Forward;
private var		_shootVector :			Vector3;
var				_weaponMaxRange :		float							= 100.0;
var				_sightGFX :				Texture2D;
var				_force :					float							= 10.0;
var				_bulletHole :			GameObject;
var				_impactSparks :		GameObject;
var				_targetAid :			GameObject;
var				_fireAudio1 :			AudioClip;
var				_fireAudioList :		AudioClip[];
var				_reloadAudio :			AudioClip;
var				_equipAudio :			AudioClip;
var				_audioSrcFire1 :		AudioSource;
var 				_fireRate1 :			float							= 0.5;
private var 	_nextFire1 :			float							= 0.0;
var				_gunLayer :				int							= -1;
var				_1stShootFrom :		Transform;
var				_1stShootTo :       Transform;
var				_muzzleBlast :			GameObject;
var				_muzzleBlastLight :	Light;
var				_blastInsensity :		float							= 0.5;
var 				_empty :					boolean						= false;
// this is linked to a singleton controlled data bar
var				dataController :		DataController;
// offset for when player is looking down
var				_handsNormalPos :		Vector3;
var				_lookDownOffset :		Vector3;
// VARIABLES for RAYCAST SHOOTING
private var		_hit :					RaycastHit;
private var		_gunSightRect :		Rect;
private var		_weaponInUse :			boolean						= false;
private var		_firstSetUp :			boolean						= true;

//---
//
//---
function Awake ()
{
	_shootVector = ReturnShootDirection();
}


//---
//
//---
function Start ()
{	
	// setup animation layers
	//if( _animator.animation[("fire-"+_weaponsName)] != null ) _animator.animation[("fire-"+_weaponsName)].layer = 2;
	//if( _animator.animation[("empty-"+_weaponsName)] != null ) _animator.animation[("empty-"+_weaponsName)].layer = 3;
	//if( _animator.animation[("reload-"+_weaponsName)] != null ) _animator.animation[("reload-"+_weaponsName)].layer = 1;
	//if( _animator.animation[("change-"+_weaponsName)] != null ) _animator.animation[("change-"+_weaponsName)].layer = 2;
	//if( _animator.animation[("activate-"+_weaponsName)] != null ) _animator.animation[("activate-"+_weaponsName)].layer = 2;
	//if( _animator.animation[("deactivate-"+_weaponsName)] != null ) _animator.animation[("deactivate-"+_weaponsName)].layer = 2;
	
	UseWeapon();
}


//---
//
//---
function LateUpdate()
{
	if (_muzzleBlastLight) {
		if ( _muzzleBlastLight.intensity > 0 ) _muzzleBlastLight.intensity -= 0.05;
	}
	
	if ( _muzzleBlast ) {
		_muzzleBlast.SetActive((_muzzleBlastLight.intensity <= 0)?false:true);
	}
}


//---
//
//---
function UseWeapon ()
{
	_weaponInUse = true;
	//gameObject.layer = _gunLayer;
	//for (var child : Transform in transform)
	//{
	//    child.gameObject.layer = _gunLayer;
	//}
	var rigidbody : Rigidbody = GetComponent(Rigidbody);
	if (rigidbody) Destroy ( rigidbody );
	var collider : Collider = GetComponent(Collider);
	if (collider) Destroy ( collider );
    
    // first setup of weapon
    if (_firstSetUp)
    {
    	dataController.initialized = true;
    	dataController.max = _maxAmmo;
		dataController.current = dataController.max;
		_firstSetUp = false;
	}
			
	if( _equipAudio ) _audioSrcFire1.PlayOneShot( _equipAudio );
		
	//--- should the weapon be hidden? (workaround for missing hands only FBX)
	//if( _hideWeapon )
	//{
	//	if( _weapon ) _weapon.SetActiveRecursively( false );
	//}
}


//---
//
//---
function DropWeapon ()
{
	_weaponInUse = false;
	//Debug.Log( "Physically Drop this Weapon: " + name );
	// instantiate pickup copy of weapon
	gameObject.SetActive(false);
}


//---
//
//---
function SwitchWeapon ( clip:String )
{
	_weaponInUse = false;
	var str:String = clip + _weaponsName;
	//Debug.Log( "Switching this Weapon: " + name );
	
	// play deactivate aimation
	//if( _animator.animation[str] != null )
	//{
	//	_animator.animation.CrossFade (str);
	//}
	
	//if( _equipAudio ) UPSound.PlayOneShot( _equipAudio );
	//if( _equipAudio ) _audioSrcFire1.PlayOneShot( _equipAudio );

	yield WaitForSeconds (0.5);
	gameObject.SetActive(false);
}


//---
//
//---
function FirePrimary( fire:String, empty:String )
{
	if ( NextShot() )
	{
		//--- if no ammo, play empty magazine sound
		if( _usesAmmo && ( dataController.current <= 0 ) )
		{
			if (_emptyMagazineSnd) {
				_audioSrcFire1.clip = _emptyMagazineSnd;
				_audioSrcFire1.Play ();
			}
		}

		if (dataController.current > 0)
		{
			//--- don't fire if _burstMax has been reached
			if( BurstPause() ) return;
			
			if ( _1stShootFrom && _1stShootTo)
			{
	//			var dir:Vector3 = _1stShootFrom.TransformDirection ( _shootVector );
				var dir:Vector3 = _1stShootTo.position - _1stShootFrom.position;
				
	
				// Did we hit anything?
				if ( Physics.Raycast (_1stShootFrom.position, dir, _hit, _weaponMaxRange, _layerMask ) ) {
					var contact:Vector3  = _hit.point;
				    var rot:Quaternion = Quaternion.FromToRotation( Vector3.up, _hit.normal );
					// Apply a force to the rigidbody we hit
					if ( _hit.rigidbody ) _hit.rigidbody.AddForceAtPosition ( _force * dir, contact );
					
					if (_hit.transform.tag.IndexOf("Interact") == -1) {
						if( (_hit.transform.tag != "Player") && (_hit.transform.tag != "NoBulletHoles") && (_hit.transform.tag != "Untagged") ) {
							// clone a temp marker
							if ( _bulletHole ) {
								var tr:GameObject = Instantiate ( _bulletHole, contact, rot );
								tr.SendMessage ( "SurfaceType", _hit );
								tr.transform.parent = _hit.transform; // parent to hit object so the bullet holes move with object
							}
						}
					}
					if (_impactSparks && _hit.transform.tag != "Player")
                    {
						Instantiate ( _impactSparks, contact, rot );
					}
					
					
					var actualdamage : float = _damage;
					if (accuracy == accuracy.Poor) actualdamage = actualdamage / 2;
					else if (accuracy == accuracy.Medium) actualdamage = actualdamage / 1.5f;
					
                    var damageSource : DamageSource = new DamageSource();
                    damageSource.damageAmount = actualdamage;
                    damageSource.fromPosition = _1stShootFrom.position;
                    damageSource.appliedToPosition = contact;
                    damageSource.sourceObject = _weaponOwner;
                    damageSource.sourceObjectType = DamageSource.DamageSourceObjectType.Player;
                    damageSource.sourceType = DamageSource.DamageSourceType.GunFire;
					
					_hit.collider.SendMessageUpwards("ApplyDamage", damageSource, SendMessageOptions.DontRequireReceiver);
				}
				
			}
			// has sound fx so play it
			if ( _fireAudioList.Length ) {
				var audioChoice : int = Random.Range( 0, _fireAudioList.Length );
				_audioSrcFire1.PlayOneShot( _fireAudioList[audioChoice] );
			}
			
			if( _usesAmmo )
			{
				_muzzleBlast.SetActive(true);
				_muzzleBlastLight.intensity = _blastInsensity;
			}
						
			// reduce ammo
			if( _usesAmmo )
			{
				dataController.current --;
				_burstCount++;
			}
			
		}
	}
}


//---
//
//---
function FirePrimaryStop( fire : String )
{
	BurstReset( "fire stop" );
}


//---
// return true if bursting should be paused
//---
function BurstPause() : boolean
{
	var burstDontFire : boolean = false;

	if( ( _burstMax > 0 ) && ( _burstDelay > 0.0 ) )
	{
		//Debug.Log( "burst count " + _burstCount );
		if( _burstCount == _burstMax )
		{		
			burstDontFire = true;
		
			if( _burstTimer > 0.0 )
			{
				if( Time.time > _burstTimer )
				{
					BurstReset( "timer expired" );
					burstDontFire = false;
				}
			}
			else
			{
				_burstTimer = Time.time + _burstDelay;
				//Debug.Log( "set burst timer" );
			}
		}
	}
	
	return( burstDontFire );
}


//---
//
//---
function BurstReset( reason : String )
{
	_burstTimer = 0.0;
	_burstCount = 0;
	//Debug.Log( "Burst reset: " + reason );
}


//---
//
//---
function NextShot () : boolean
{
	var canFire = false;
	
	if (Time.time > _nextFire1) {
		canFire = true;
		_nextFire1 = Time.time + _fireRate1;
	}
	
	return canFire;
}


//---
//
//---
function SetBulletShootFrom ( sft:Transform )
{
	_1stShootFrom = sft;
}


//---
//
//---
function SetIgnoreColliders ( cols:Collider[] )
{
	_ignoreColliders = cols;
}


//---
//
//---
function ReloadWeaponStart()
{
//	if( _reloadAudio ) UPSound.PlayOneShot( _reloadAudio );
	if( _reloadAudio ) _audioSrcFire1.PlayOneShot( _reloadAudio );
}


//---
//
//---
function ReloadWeapon ()
{
	dataController.max = _maxAmmo;
	dataController.current = dataController.max;
	_empty = false;
	BurstReset( "weapon reloaded" );
}


//---
//
//---
function ReturnShootDirection () : Vector3
{
	var vec:Vector3 = Vector3.forward;
	switch (_shootDirection)
	{
		case AIShootDirection.Right:
			vec = Vector3.right;
		break;
		case AIShootDirection.Left:
			vec = Vector3.left;
		break;
		case AIShootDirection.Forward:
			vec = Vector3.forward;
		break;
		case AIShootDirection.Backward:
			vec = Vector3.back;
		break;
	}
	return vec;
}