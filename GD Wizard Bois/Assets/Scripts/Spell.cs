using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public enum TargetType
    {
        Single,
        Party
    }

    public string SpellName;
    public List<StatusEffect> StatusEffectsAppliedToTarget;
    public int Damage;
    public TargetType Type;

    [SerializeField] private int[] patternArray;

    public bool PatternArrayMatches(int[] otherPatternArray)
    {
        if (otherPatternArray == null) return false;
        return patternArray.SequenceEqual(otherPatternArray);
    }

    public override bool Equals(object other)
    {
        Spell otherSpell = (Spell) other;
        if (otherSpell == null) return false;

        return otherSpell.SpellName == this.SpellName && otherSpell.Damage == this.Damage &&
                otherSpell.Type == this.Type;
    }
}
