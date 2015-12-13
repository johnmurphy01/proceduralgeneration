using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction("Update Attack Position")]
public class AIUpdateAttackPosition : RAINAction
{
    public AIUpdateAttackPosition()
    {
        actionName = "AIUpdateAttackPosition";
    }

    public override ActionResult Execute(AI ai)
    {
		AttackHarness attackHarness = ai.WorkingMemory.GetItem<AttackHarness>("attacktargetharness");
		AttackHarness waitHarness = ai.WorkingMemory.GetItem<AttackHarness>("waitharness");
		if ((attackHarness == null) && (waitHarness == null))
			return ActionResult.FAILURE;
		
		AttackHarness harness = null;
		int slot = -1;
		if ((attackHarness != null) && (ai.WorkingMemory.ItemExists("attacktargetharnessslot")))
		{
			harness = attackHarness;
			slot = ai.WorkingMemory.GetItem<int>("attacktargetharnessslot");
		}
		if ((harness == null) || (slot < 0))
		{
			if ((waitHarness != null) && (ai.WorkingMemory.ItemExists("waitharnessslot")))
			{
				harness = waitHarness;
				slot = ai.WorkingMemory.GetItem<int>("waitharnessslot");				
			}
		}

		if ((harness == null) || (slot < 0))
			return ActionResult.FAILURE;
		
		
		Vector3 attackpos = harness.GetAttackPosition(slot);
		ai.WorkingMemory.SetItem<Vector3>("attackposition", attackpos);

		ai.WorkingMemory.SetItem<bool>("run", false);
		
		if (!ai.Navigator.OnGraph(attackpos))
			return ActionResult.FAILURE;
		
		float distance = (attackpos - ai.Body.transform.position).magnitude;
		if (distance > 2.5f)
			ai.WorkingMemory.SetItem<bool>("run", true);

		return ActionResult.SUCCESS;
    }
}