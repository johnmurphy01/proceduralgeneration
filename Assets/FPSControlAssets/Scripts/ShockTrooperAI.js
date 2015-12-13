//import Aether;
//import Legion.Core;
import RAIN.Core;
import RAIN.Animation;

var _gunLogic : AIGunLogic;
//var _rival : Rival;
var _dataController : DataController;
var _player : GameObject;
var meleeDamage : float = 10.0f;
var hideWhenDead : boolean = false;
var _ai : AIRig;
var _mecanimAnimator : UnityEngine.Animator;

function Start()
{
	_player = GameObject.Find("Player");
	_ai = gameObject.GetComponentInChildren.<AIRig>();
	_dataController = gameObject.GetComponent("DataController");
	_mecanimAnimator = _ai.AI.Animator.Animator;	
		
}

function Update()
{	
	
	_ai.AI.WorkingMemory.SetItem.<float>("health", _dataController.current);
	_ai.AI.WorkingMemory.SetItem.<float>("ammo", _gunLogic.dataController.current);
	
	var shootAt : GameObject = _ai.AI.WorkingMemory.GetItem.<GameObject>("enemytarget");
	if (shootAt != null)
	{
		_gunLogic._1stShootTo = shootAt.transform;
		
		// if we're aiming, set the current aim angle
		if (_mecanimAnimator.GetBool("Aim"))
		{
			var aiPos : Vector3 = _ai.AI.Body.transform.position;
			var targetPos : Vector3 = shootAt.transform.position;
			
			var xdist : float = aiPos.x - targetPos.x;
			var yA : float = aiPos.y - targetPos.y; // + 0.75f; // add height to account for AI shoulder height
			var z : float = aiPos.z - targetPos.z;
			xdist = Mathf.Sqrt(xdist*xdist + z*z);
			
			yA = Mathf.Atan(-yA/xdist) * Mathf.Rad2Deg;
			_mecanimAnimator.SetFloat("GunAngle", yA);
		}
		else
		{
			_mecanimAnimator.SetFloat("GunAngle", 0.0f);
		}
	}
	else
	{
		_mecanimAnimator.SetFloat("GunAngle", 0.0f);
	}
}

function ApplyDamage(damageSource : DamageSource)
{
	if ((damageSource.sourceType == DamageSource.DamageSourceType.GunFire) &&
	    (damageSource.sourceObjectType != DamageSource.DamageSourceObjectType.Obstacle))
    {
		_ai.AI.WorkingMemory.SetItem.<boolean>("gothit", true);

		transform.Rotate(Vector3.up, -45f);
		var testforward:Vector3 = transform.forward;
		var testright:Vector3 = transform.right;
		transform.Rotate(Vector3.up, 45f);		
		
		if (Vector3.Dot(testforward, damageSource.fromPosition) > 0)
		{
			if (Vector3.Dot(testright, damageSource.fromPosition) > 0)
				_mecanimAnimator.SetInteger("HitDirection", 4); //left
			else
				_mecanimAnimator.SetInteger("HitDirection", 1); //front
		}
		else
		{
			if (Vector3.Dot(testright, damageSource.fromPosition) > 0)
				_mecanimAnimator.SetInteger("HitDirection", 3); //back
			else
				_mecanimAnimator.SetInteger("HitDirection", 2); //right
		}

		var verticalDistance : float;
		verticalDistance = damageSource.appliedToPosition.y - _ai.AI.Body.transform.position.y;
		if (verticalDistance > 1f)
			_mecanimAnimator.SetInteger("HitHeight", 1);
		else if (verticalDistance > 0.5f)
			_mecanimAnimator.SetInteger("HitHeight", 2);
		else
			_mecanimAnimator.SetInteger("HitHeight", 3);
			
		_ai.AI.Animator.StartState("Hit");
					
		_ai.AI.WorkingMemory.SetItem.<GameObject>("enemytarget", damageSource.sourceObject);
	}
	_dataController.current -= damageSource.damageAmount;
}

//@Aether.MessageHandler("FirePrimaryWeapon")
function FirePrimaryWeapon()
{
	_gunLogic.FirePrimary("fire-", "empty-");
}
		
//@Aether.MessageHandler("FirePrimaryWeaponStop")
function FirePrimaryWeaponStop()
{
	_gunLogic.FirePrimaryStop("fire-");
}	

//@Aether.MessageHandler("ReloadPrimaryWeapon")
function ReloadPrimaryWeapon()
{
	_gunLogic.ReloadWeapon();
}

//@Aether.MessageHandler("Die")
function Die()
{
	if (hideWhenDead)
	{
		gameObject.SetActive(false);
	}
	else
	{
		var colliders : Collider[] = gameObject.GetComponentsInChildren.<Collider>();
		for (var i:int = 0; i < colliders.Length; i++)
		{
			if ((colliders[i] != null) && !colliders[i].isTrigger )
			{
				if( typeof( colliders[i] ) == CharacterController )
				{
					colliders[i].gameObject.SetActive(false);
				}
				else
				{
					GameObject.DestroyImmediate(colliders[i]);
				}
			}
		}		
	}
}

//@Aether.MessageHandler("MeleeAttack")
function MeleeAttack()
{
	var target : GameObject = _ai.AI.WorkingMemory.GetItem.<GameObject>("meleetarget");
	var damageSource : DamageSource = new DamageSource();
	damageSource.appliedToPosition = target.transform.position;
	damageSource.damageAmount = meleeDamage;
	
	damageSource.fromPosition = _ai.AI.Body.transform.position;
	damageSource.sourceObject = _ai.AI.Body;
	damageSource.sourceObjectType = DamageSource.DamageSourceObjectType.AI;
	damageSource.sourceType = DamageSource.DamageSourceType.MeleeAttack;
	
	target.SendMessage("ApplyDamage", damageSource, SendMessageOptions.DontRequireReceiver);
}
