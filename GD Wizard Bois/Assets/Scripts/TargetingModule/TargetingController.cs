using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TargetingController : NetworkBehaviour
{
    public Button bossTargetButton;
    public BossEnemy boss;

    public WizardTargetDisplay[] TargetDisplays;

    // The Wizard object of the localplayer will always be at position [0]
    private Wizard[] allWizards;

    private PlayerConnection playerConnectionObject;

    public void setPlayerConnectionObject(PlayerConnection connectionObject)
    {
        playerConnectionObject = connectionObject;
    }

    private void Start()
    {
        foreach (WizardTargetDisplay wizardTargetDisplay in TargetDisplays)
        {
            wizardTargetDisplay.OnWizardTargetClicked.AddListener(onWizardTargetSelected);
        }
    }

    public void UpdateTargetsDisplays()
    {
        SpellStatusDisplay spellStatusDisplay = GameObject.FindGameObjectWithTag("SpellStatus").GetComponent<SpellStatusDisplay>();
        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizard");

        allWizards = new Wizard[wizards.Length];

        for (int i = 0, j = 1; i < wizards.Length && j < TargetDisplays.Length; i++, j++)
        {
            Wizard wizScript = wizards[i].GetComponent<Wizard>();

            if (wizScript.hasAuthority)
            {
                TargetDisplays[0].SetTarget(wizScript);
                allWizards[0] = wizScript;
                spellStatusDisplay.wizardWithStatus = allWizards[0];
                j--;
            }
            else
            {
                TargetDisplays[j].SetTarget(wizScript);
                allWizards[j] = wizScript;
            }
        }
    }

    public Wizard GetWizardById(int id)
    {
        foreach (Wizard wizard in allWizards)
        {
            if (wizard.WizardId == id) return wizard;
        }

        return null;
    }

    public void onBossTargetSelected()
    {
        if (allWizards[0].getLockedSpell() != null && !allWizards[0].castCooldown)
        {
            playerConnectionObject.CmdWizardUseSpellOnBoss(allWizards[0].WizardId);
        }
    }

    private void onWizardTargetSelected(int wizId)
    {
        Spell lockedSpell = allWizards[0].getLockedSpell();

        if (lockedSpell != null && !allWizards[0].castCooldown)
        {
            if (lockedSpell.Type == Spell.TargetType.Party)
            {
                int[] wizIds = new int[allWizards.Length];

                for (var i = 0; i < allWizards.Length; i++)
                {
                    wizIds[i] = allWizards[i].WizardId;
                }

                playerConnectionObject.CmdWizardUseSpellOnWizards(wizIds, allWizards[0].WizardId);
            }
            else if (lockedSpell.Type == Spell.TargetType.Single)
            {
                playerConnectionObject.CmdWizardUseSpellOnWizards(new int[] {wizId}, allWizards[0].WizardId);
            }
        }
    }
}