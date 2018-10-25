using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    public GameObject WizardPrefab;

    private TargetingController targetingController;

    private void Start()
    {
        targetingController = GameObject.FindGameObjectWithTag("TargetController").GetComponent<TargetingController>();

        if (isLocalPlayer)
        {
            targetingController.setPlayerConnectionObject(this);
            CmdSpawnLocalPlayerWizard(this.transform.position);
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

        RpcUpdateTargeting();
    }

    [Command]
    public void CmdWizardUseSpellOnWizards(int[] targetIds, int casterId)
    {
        Wizard caster = targetingController.GetWizardById(casterId);
        Spell spell = caster.getLockedSpell();

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
            caster.unlockSpell();
        }
    }

    [Command]
    public void CmdWizardUseSpellOnBoss(int casterId)
    {
        Wizard caster = targetingController.GetWizardById(casterId);
        Spell spell = caster.getLockedSpell();

        if (spell != null && !caster.castCooldown)
        {
            targetingController.boss.healthScript.TakeDamage(spell.Damage);

            caster.castCooldown = true;
            caster.unlockSpell();
        }
    }

    ///////////////////////
    ////
    [ClientRpc]
    public void RpcUpdateTargeting()
    {
        targetingController.NewWizardConnected();
    }
}