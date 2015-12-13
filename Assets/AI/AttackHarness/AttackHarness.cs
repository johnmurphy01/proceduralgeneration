using UnityEngine;
using System.Collections.Generic;
using RAIN.Navigation;

public class AttackHarness : MonoBehaviour 
{
	public const int cnstMaxPossibleAttackers = 30;
	
	public string harnessName;
	public int maxAttackers = 6;
	public float attackDistance = 1f;
	public bool rotatesWithObject = false;
	
	public bool displayVisualization = false;
	public Color emptyColor = Color.green;
	public Color occupiedColor = Color.red;
	
	[System.NonSerialized]
	public GameObject[] attackers = new GameObject[cnstMaxPossibleAttackers];
	
	void Start () 
	{
		maxAttackers = Mathf.Clamp(maxAttackers, 0, cnstMaxPossibleAttackers);
		Clear();
	}
		
	void Clear()
	{
		for (int i = 0; i < attackers.Length; i++)
			attackers[i] = null;
	}
	
	public void VacateAttack(GameObject attacker)
	{
		for (int i = 0; i < attackers.Length; i++)
			if (attackers[i] == attacker) 
				attackers[i] = null;
	}
	
	public bool OccupyClosestAttackSlot(GameObject attacker, out int attackSlot, RAINNavigator navigator)
	{
		for (int i = 0; i < maxAttackers; i++)
			if (attackers[i] == attacker)
			{
				attackSlot = i;
				return true;
			}
		
		List<int> openList = new List<int>();
		for (int i = 0; i < maxAttackers; i++)
			if (attackers[i] == null)
				openList.Add(i);
		
		float bestDistance = float.MaxValue;
		int bestSlot = -1;
		for (int i = 0; i < openList.Count; i++)
		{
			Vector3 attackPosition = GetAttackPosition(openList[i]);
			if ((navigator != null) && (!navigator.OnGraph(attackPosition)))
				continue;
			
			float distance = (attacker.transform.position - attackPosition).magnitude;
			if (distance < bestDistance)
			{
				bestDistance = distance;
				bestSlot = openList[i];
			}
		}
		
		attackSlot = bestSlot;
		if (bestSlot < 0)
			return false;
		
		attackers[bestSlot] = attacker;
		return true;
	}
	
	public bool OccupyAttackSlot(GameObject attacker, int attackSlot)
	{
		if ((attackSlot < 0) || (attackSlot >= maxAttackers))
			return false;
		
		if (attackers[attackSlot] == null)
		{
			attackers[attackSlot] = attacker;
			return true;
		} 
		else if (attackers[attackSlot] == attacker) 
		{
			return true;
		}
		
		return false;
	}
	
	public Vector3 GetAttackPosition(int attackSlot)
	{
		if ((attackSlot < 0) || (attackSlot >= maxAttackers))
		{
			return gameObject.transform.position + gameObject.transform.forward * attackDistance;
		}
		
		float angle = (360f / maxAttackers) * (float)attackSlot;
		if (rotatesWithObject)
			angle += gameObject.transform.rotation.eulerAngles.y;
		
		Quaternion saved = gameObject.transform.rotation;
		gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

		Vector3 returnValue = gameObject.transform.position + gameObject.transform.forward * attackDistance;
		gameObject.transform.rotation = saved;
		
		return returnValue;
	}
	
	void OnDrawGizmos()
	{
		if (!displayVisualization)
			return;
		
		if (maxAttackers < 1)
			return;
		
		for (int i = 0; i < maxAttackers; i++)
		{
			if (attackers[i] == null)
				Gizmos.color = emptyColor;
			else
				Gizmos.color = occupiedColor;
			
	        Gizmos.matrix = Matrix4x4.identity;
			Gizmos.DrawWireCube(GetAttackPosition(i), new Vector3(0.2f, 0.2f, 0.2f));		
		}
	}
}
