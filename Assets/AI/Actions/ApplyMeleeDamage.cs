using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class ApplyMeleeDamage : RAINAction
{
    public ApplyMeleeDamage()
    {
        actionName = "ApplyMeleeDamage";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		ai.Body.SendMessage("MeleeAttack");
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}