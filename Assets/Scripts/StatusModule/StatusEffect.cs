using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    public enum EffectType
    {
        Weakness, // Lower Attack
        Strength, // Increase Attack
        DefenceBroken, // Lower Defence
        Fortified // Increase Defence
    }

//    public RawImage EffectIcon;
    public EffectType TypeEffect;
    public int TurnTickCount;
}
