#pragma strict

import RAIN.Core;

var _ai : AIRig;
var _dataController : DataController;
var _hungry: float;

function Start () {

	_ai = gameObject.GetComponentInChildren.<AIRig>();
	_dataController = gameObject.GetComponent.<DataController>();
	
	_ai.AI.WorkingMemory.SetItem.<float>("hungry", 15f);
	_ai.AI.WorkingMemory.SetItem.<boolean>("dead", false);
	_ai.AI.WorkingMemory.SetItem.<boolean>("foundfood", false);
	_ai.AI.WorkingMemory.SetItem.<boolean>("gothit", false);
}

function Update () {

	_ai.AI.WorkingMemory.SetItem.<float>("health", _dataController.current);
	_ai.AI.WorkingMemory.SetItem.<float>("currenttime", Time.time);
	_hungry = _ai.AI.WorkingMemory.GetItem.<float>("hungry");
}

function ApplyDamage(damageSource : DamageSource)
{
	if ((damageSource.sourceType == DamageSource.DamageSourceType.GunFire) &&
	    (damageSource.sourceObjectType != DamageSource.DamageSourceObjectType.Obstacle))
    {
		_ai.AI.WorkingMemory.SetItem.<boolean>("gothit", true);
		_ai.AI.WorkingMemory.SetItem.<GameObject>("enemytarget", damageSource.sourceObject);
	}
	
	_dataController.current -= damageSource.damageAmount;
}