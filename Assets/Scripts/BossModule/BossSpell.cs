using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Spell", menuName = "BossSpell")]
public class BossSpell : ScriptableObject
{
	public enum TargetType
	{
		Self,
		Single,
		Party
	}

	public List<StatusEffect> StatusEffectsAppliedToTarget;
	public int Damage;
	public TargetType Type;
}
