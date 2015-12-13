using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class SetMoveTarget : RAINAction
{
    public SetMoveTarget()
    {
        actionName = "SetMoveTarget";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		GameObject player = ai.WorkingMemory.GetItem<GameObject>("attacktarget");
		
		ai.WorkingMemory.SetItem<Vector3>("lastSeenPlayerPos", player.transform.position);
		
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}