using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    public GameObject WizardPrefab;

    private TargetingController targetingController;
    private Wizard myWiz;

    private void Start()
    {
        targetingController = GameObject.FindGameObjectWithTag("TargetController").GetComponent<TargetingController>();

        if (isLocalPlayer)
        {
            targetingController.setPlayerConnectionObject(this);
            CmdSpawnLocalPlayerWizard(this.transform.position);
        }
    }

    public void CastSpell(int spellIndex)
    {
        if (myWiz.castCooldown || spellIndex < 0) return;
        if (targetingController.GetTargetId() == -1)
        {
            CmdWizardUseSpellOnBoss(myWiz.WizardId, spellIndex);
        }
        else
        {
            if (myWiz.SpellBook[spellIndex].Type == Spell.TargetType.Party)
            {
                CmdWizardUseSpellOnWizards(targetingController.GetAllWizardIds(), myWiz.WizardId, spellIndex);
            }
            else
            {
                CmdWizardUseSpellOnWizards(new int[] {targetingController.GetTargetId()}, myWiz.WizardId, spellIndex);
            }
        }

    }

    ///////////////////////
    ////
    [Command]
    private void CmdSpawnLocalPlayerWizard(Vector3 spawnPosition)
    {
        GameObject spawnedWizard =
            Instantiate(WizardPrefab, spawnPosition, Quaternion.identity, this.transform);
        NetworkServer.SpawnWithClientAuthority(spawnedWizard, this.gameObject);

        RpcUpdateTargetingController();
    }

    [Command]
    public void CmdWizardUseSpellOnWizards(int[] targetIds, int casterId, int spellIndex)
    {
        Wizard caster = targetingController.GetWizardById(casterId);
//        Spell spell = caster.getLockedSpell();
        Spell spell = caster.SpellBook[spellIndex];

        // TODO: organise this code
        // TODO: apply status effects
        if (spell != null && !caster.castCooldown)
        {
            if (spell.Type == Spell.TargetType.Single)
            {
                Wizard target = targetingController.GetWizardById(targetIds[0]);

                target.healthScript.TakeDamage(spell.Damage);
            }
            else if (spell.Type == Spell.TargetType.Party)
            {
                for (int i = 0; i < targetIds.Length; i++)
                {
                    Wizard target = targetingController.GetWizardById(targetIds[i]);
                    target.healthScript.TakeDamage(spell.Damage);
                }
            }

            caster.castCooldown = true;
//            caster.unlockSpell();
        }
    }

    [Command]
    public void CmdWizardUseSpellOnBoss(int casterId, int spellIndex)
    {
        Wizard caster = targetingController.GetWizardById(casterId);
//        Spell spell = caster.getLockedSpell();
        Spell spell = caster.SpellBook[spellIndex];

        if (spell != null && !caster.castCooldown)
        {
            targetingController.boss.healthScript.TakeDamage(spell.Damage);

            caster.castCooldown = true;
//            caster.unlockSpell();
        }
    }

    ///////////////////////
    ////
    [ClientRpc]
    public void RpcUpdateTargetingController()
    {
        targetingController.NewWizardConnected();
        myWiz = targetingController.GetAllWizards()[0];
//        Debug.Log(myWiz);
        myWiz.SetPlayerObject(this);
    }
}