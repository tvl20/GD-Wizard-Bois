using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public enum TargetType
    {
        Single,
        Party
    }

    public List<StatusEffect> StatusEffectsAppliedToTarget;
    public int Damage;
    public TargetType Type;
    public TouchPatternInput.UniquePatterns SpellPattern;
}
