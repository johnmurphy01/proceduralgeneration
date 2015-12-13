using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Serialization;
using RAIN.Motion;

[RAINSerializableClass]
[RAINElement("Shock Trooper Motor")]
public class ShockTrooperMotor : MecanimMotor
{	
	private CharacterController _characterController = null;
	
	public override void AIInit ()
	{
		base.AIInit ();
		if (AI.Body != null)
			_characterController = AI.Body.GetComponent<CharacterController>();
	}
	public override void ApplyMotionTransforms ()
	{
		if (_characterController == null)
		{
			Debug.LogWarning("No character controller found");
			base.ApplyMotionTransforms();
		}
		else
		{
			AI.Kinematic.UpdateTransformData(AI.DeltaTime);
			UpdateMecanimParameters();
			
			_characterController.SimpleMove(AI.Kinematic.Velocity);
			AI.Body.transform.rotation = Quaternion.Euler(AI.Kinematic.Orientation);
		}
	}
}
