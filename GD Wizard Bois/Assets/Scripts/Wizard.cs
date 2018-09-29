using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(DamageAble), typeof(HealthStatus))]
[RequireComponent(typeof(DamageAble))]
public class Wizard : NetworkBehaviour
{
    public List<Spell> SpellBook;

    [SyncVar] public int WizardId;
    [SyncVar] public bool castCooldown = false;

    public DamageAble healthScript;
//	private HealthStatus statusEffectsScript;

    [SyncVar] private int lockedSpellIndex = -1;

    public Spell getLockedSpell()
    {
        if (lockedSpellIndex > -1 && lockedSpellIndex < SpellBook.Count)
        {
            return SpellBook[lockedSpellIndex];
        }

        return null;
    }


//	private Spell lockedSpell = null;
//	public Spell getLockedSpell()
//	{
//		return lockedSpell;
//	}

    private void Awake()
    {
        healthScript = GetComponent<DamageAble>();
    }

    // Start is executed before authority is given to this gameobject
    public override void OnStartAuthority()
    {
        int wizId = int.Parse(GetComponent<NetworkIdentity>().netId.ToString());
//			Debug.Log(wizId + " will be sent to server to set ID");
        CmdSetWizardId(wizId);

        GameObject.FindGameObjectWithTag("Input").GetComponent<TouchPatternInput>().onFinishedPattern
            .AddListener(lockSpell);
//			Debug.Log("input connected to wizard");

        GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>().onTimerTick
            .AddListener(CmdResetCooldown);
    }

//    private void Start()
//    {
//        if (hasAuthority)
//        {
//			int wizId = int.Parse(GetComponent<NetworkIdentity>().netId.ToString());
////			Debug.Log(wizId + " will be sent to server to set ID");
//			CmdSetWizardId(wizId);
//
//            GameObject.FindGameObjectWithTag("Input").GetComponent<TouchPatternInput>().onFinishedPattern
//                .AddListener(lockSpell);
////			Debug.Log("input connected to wizard");
//
//            GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>().onTimerTick
//                .AddListener(CmdResetCooldown);
//        }
//    }

    public void unlockSpell()
    {
//		lockedSpell = null;
        CmdLockSpell(-1);
    }

    private void lockSpell(TouchPatternInput.UniquePatterns spellPattern)
    {
        if (castCooldown || spellPattern == TouchPatternInput.UniquePatterns.None) return;


        int spellIndex = -1;
        for (int i = 0; i < SpellBook.Count; i++)
        {
            if (SpellBook[i].SpellPattern == spellPattern)
            {
//				lockedSpell = spell;
                spellIndex = i;
                break;
            }
        }

        if (spellIndex > -1)
            CmdLockSpell(spellIndex);
    }

    //////////////////////
    ////
    [Command]
    private void CmdSetWizardId(int id)
    {
        this.WizardId = id;
    }

    [Command]
    private void CmdLockSpell(int spellIndex)
    {
        lockedSpellIndex = spellIndex;
    }

    [Command]
    private void CmdResetCooldown()
    {
        castCooldown = false;
    }
}