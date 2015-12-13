using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Perception.Sensors;

[RAINAction("Choose Attack Position")]
public class AIChooseAttackPosition : RAINAction
{	
    public AIChooseAttackPosition()
    {
        actionName = "AIChooseAttackPosition";
    }

	public override void Start (AI ai)
	{	
		base.Start (ai);
		
		AttackHarness meleeHarness = null;
		AttackHarness waitHarness = null;
		
		GameObject attackTarget = ai.WorkingMemory.GetItem<GameObject>("attacktarget");
		if (attackTarget != null)
		{
			AttackHarness[] harnesses = attackTarget.GetComponentsInChildren<AttackHarness>();
			for (int i = 0; i < harnesses.Length; i++)
			{
				if ((harnesses[i] != null) && (harnesses[i].harnessName == "attack"))
					meleeHarness = harnesses[i];
				if ((harnesses[i] != null) && (harnesses[i].harnessName == "wait"))
					waitHarness = harnesses[i];
			}
		}		
		ai.WorkingMemory.SetItem<AttackHarness>("attacktargetharness", meleeHarness);
		ai.WorkingMemory.SetItem<AttackHarness>("waitharness", waitHarness);
	}
	
    public override ActionResult Execute(AI ai)
    {
		AttackHarness meleeHarness = ai.WorkingMemory.GetItem<AttackHarness>("attacktargetharness");
		if (meleeHarness != null)
			meleeHarness.VacateAttack(ai.Body);
		
		AttackHarness waitHarness = ai.WorkingMemory.GetItem<AttackHarness>("waitharness");
		if (waitHarness != null)
			waitHarness.VacateAttack(ai.Body);
		
		int tAttackSlot = -1;
		bool foundSlot = false;
		if (meleeHarness != null)
		{
			foundSlot = meleeHarness.OccupyClosestAttackSlot(ai.Body, out tAttackSlot, null);
			if (foundSlot)
		        ai.WorkingMemory.SetItem<int>("attacktargetharnessslot", tAttackSlot);
			else
				ai.WorkingMemory.SetItem<int>("attacktargetharnessslot", -1);
		}
		if (!foundSlot)
	        ai.WorkingMemory.SetItem<int>("attacktargetharnessslot", -1);

		if ((!foundSlot) && (waitHarness != null))
		{
			tAttackSlot = -1;
			foundSlot =  waitHarness.OccupyClosestAttackSlot(ai.Body, out tAttackSlot, null);
			if (foundSlot)
		        ai.WorkingMemory.SetItem<int>("waitharnessslot", tAttackSlot);
			else
				ai.WorkingMemory.SetItem<int>("waitharnessslot", -1);
		}
		else
		{
	        ai.WorkingMemory.SetItem<int>("waitharnessslot", -1);
		}
		
		if (!foundSlot)
			return ActionResult.FAILURE;
		
        return ActionResult.RUNNING;
    }
	
	public override void Stop (AI ai)
	{
		AttackHarness harness = ai.WorkingMemory.GetItem<AttackHarness>("attacktargetharness");
		if (harness != null)
			harness.VacateAttack(ai.Body);
		
		harness = ai.WorkingMemory.GetItem<AttackHarness>("waitharness");
		if (harness != null)
			harness.VacateAttack(ai.Body);
		
		ai.WorkingMemory.RemoveItem("attacktargetharness");
		ai.WorkingMemory.RemoveItem("attacktargetharnessslot");
		ai.WorkingMemory.RemoveItem("waitharness");
		ai.WorkingMemory.RemoveItem("waitharnessslot");
		
		base.Stop (ai);
	}
}