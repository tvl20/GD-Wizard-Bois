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

    public bool alive = true;
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

    private void Awake()
    {
        healthScript = GetComponent<DamageAble>();
        healthScript.EventOnZeroHealth += onZeroHealthHandler;
    }

    // Start is executed before authority is given to this gameobject
    public override void OnStartAuthority()
    {
        int wizId = int.Parse(GetComponent<NetworkIdentity>().netId.ToString());
        CmdSetWizardId(wizId);

        GameObject.FindGameObjectWithTag("Input").GetComponent<TouchPatternInput>().OnFinishedSpell
            .AddListener(lockSpell);

        GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>().onTimerTick
            .AddListener(CmdResetCooldown);
    }

    public void unlockSpell()
    {
        CmdLockSpell(-1);
    }

    private void lockSpell(Spell spell)
    {
        if (castCooldown || spell == null) return;


        int spellIndex = -1;
        for (int i = 0; i < SpellBook.Count; i++)
        {
            if (SpellBook[i].Equals(spell))
            {
                spellIndex = i;
                break;
            }
        }

        if (spellIndex > -1)
            CmdLockSpell(spellIndex);
    }

    private void onZeroHealthHandler()
    {
        alive = false;
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