using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    public GameObject WizardPrefab;

    private TargetingController targetingController;

    public override void OnStartLocalPlayer()
    {
//        CmdSpawnLocalPlayerWizard(this.transform.position);
    }

    private void Start()
    {
//		if (!isLocalPlayer) return;

        targetingController = GameObject.FindGameObjectWithTag("TargetController").GetComponent<TargetingController>();
//		Debug.Log(targetingController + " targetcontroller set");


        if (isLocalPlayer)
        {
            targetingController.setPlayerConnectionObject(this);
            CmdSpawnLocalPlayerWizard(this.transform.position);
        }
    }

//	public void UseSpellOnWizards(Wizard[] targetWizards, Spell spell)
//	{
//
//	}
//
//	public void UseSpellOnBoss(Spell spell)
//	{
//
//	}

    ///////////////////////
    ////
//	private GameObject myGameObject;

    [Command]
    private void CmdSpawnLocalPlayerWizard(Vector3 spawnPosition)
    {
        GameObject spawnedWizard =
            Instantiate(WizardPrefab, spawnPosition, Quaternion.identity, this.transform);
//		myGameObject = spawnedWizard;
        NetworkServer.SpawnWithClientAuthority(spawnedWizard, this.gameObject);

//		Debug.Log("Adding another Wizard");

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

//            Debug.Log("Spell was cast");
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
        targetingController.UpdateTargetsDisplays();
    }
}