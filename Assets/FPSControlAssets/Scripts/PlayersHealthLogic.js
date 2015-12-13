//=====
//=====


import FPSControl;
//import Legion.Ontology;


var				_loadLevel :			boolean						= true;
var				_takeCollideDamage :	boolean						= true;
var				_collideThreshhold :	float							= 7.0;
var				_maxHealth :			float							= 100;
var				_defaultDamage :		float							= 1;
var				_deathMusic :			String						= "PlayerDeath";
var				_playerStateText :	GUIText;

// this is linked to a singleton controlled data bar
var				_dataController :		DataController;


private var		_isAlive :				boolean						= true;


//---
//
//---
function Start()
{
	_dataController.max = _maxHealth;
	_dataController.current = _dataController.max;
	_dataController.initialized = true;
}


//---
// been hit by something, is it mortal?
//---
function OnCollisionEnter( collision : Collision )
{
	var damage:float = collision.relativeVelocity.magnitude;
	
	//--- take collider damage? (fall damage, etc)
	if( _takeCollideDamage && ( damage > _collideThreshhold ) )
	{
//		Debug.Log( "collided with " + collision.collider.name + " with force " + damage );
                    var damageSource : DamageSource = new DamageSource();
                    damageSource.damageAmount = damage - _collideThreshhold;
                    damageSource.fromPosition = collision.transform.position;
                    damageSource.appliedToPosition = collision.transform.position;
                    damageSource.sourceObject = collision.gameObject;
                    damageSource.sourceObjectType = DamageSource.DamageSourceObjectType.Obstacle;
                    damageSource.sourceType = DamageSource.DamageSourceType.StaticCollision;
                    
		ApplyDamage( damageSource);
	}

		if (collision.gameObject.tag == "Trap")
	{
		//
	}
}


//---
// 
//---
function IsAlive() : boolean
{
	return( _isAlive );
}

function ApplyHealthAdditive(value : float)
{
    _dataController.current += value;
    AmIDead();
}

function ApplyHealth(value : float)
{
    _dataController.current = value;
    AmIDead();
}

//---
// called via SendMessage from weapons or traps raycasting
//---
function ApplyDamage( damageSource : DamageSource )
{
	//--- get quadrant of Player that was shot
	var hurtQuad : int = HurtQuadrant.FRONT;
	var hitDir : Vector3 = damageSource.appliedToPosition - transform.position;
	var left : Vector3 = transform.TransformDirection( Vector3.left );
	var forward : Vector3 = transform.TransformDirection( Vector3.forward );
	
	var forwardDist : float = Vector3.Dot( forward, hitDir );
	var sideDist : float = Vector3.Dot( left, hitDir );
	
	if( damageSource.sourceType == DamageSource.DamageSourceType.Fall )
	{
		hurtQuad = HurtQuadrant.BACK;
	}
	else if( Mathf.Abs( sideDist ) < 0.1F )
	{
		if( forwardDist < 0 )
		{
			hurtQuad = HurtQuadrant.BACK;
		}
		else
		{
			hurtQuad = HurtQuadrant.FRONT;
		}
	}
	else
	{
		if( sideDist >= 0 )
		{
			hurtQuad = HurtQuadrant.LEFT;
		}
		else
		{
			hurtQuad = HurtQuadrant.RIGHT;
		}
	}
	
	BroadcastMessage( "GotHurtQuadrant", hurtQuad );
	
	TakeDamage ( damageSource );
	// is it time to meet the maker?
	AmIDead ();
}


//---
// reduce health
//---
function TakeDamage( damageSource : DamageSource )
{
	_dataController.current -= damageSource.damageAmount;
}


//---
// is player dead?
//---
function AmIDead ()
{
	if( ( _dataController.current <= 0 ) && ( _isAlive ) )
	{
		_isAlive = false;
		
		// show red screen, ala HalfLife 2??
		if( _playerStateText != null )
		{
			_playerStateText.gameObject.SetActive(true);
		}
		MessengerControl.Broadcast( "FadeIn", _deathMusic );
		BroadcastMessage ("PlayerDied");
		
		//--- tell the AI that the player is dead
//		var decoration : Decoration = GetComponent("Decoration");
//		if( decoration != null )
//		{
//			decoration.aspect.aspectName="deadplayer";
//		}
	}
}