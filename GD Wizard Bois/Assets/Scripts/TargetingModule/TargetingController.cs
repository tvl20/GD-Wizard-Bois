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

//	private List<Wizard> targetableWizards = new List<Wizard>();

    private void Start()
    {
        foreach (WizardTargetDisplay wizardTargetDisplay in TargetDisplays)
        {
            wizardTargetDisplay.OnWizardTargetClicked.AddListener(onWizardTargetSelected);
        }
    }

    private void Update()
    {
    }

//	public void AddWizardToTargeting(Wizard wizard)
//	{
//		targetableWizards.Add(wizard);
//		fixTargetDisplays();
//	}

    public void UpdateTargetsDisplays()
    {
        SpellStatusDisplay spellStatusDisplay = GameObject.FindGameObjectWithTag("SpellStatus").GetComponent<SpellStatusDisplay>();
        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizard");

//        Debug.Log(wizards.Length);

        allWizards = new Wizard[wizards.Length];

        // TODO: UPDATE THIS TO MAKE IT MORE EFFICIENT/OPTIMIZED
//        for (int i = 0; i < wizards.Count; i++)
//        {
//            Wizard wizScript = wizards[i].GetComponent<Wizard>();
//
//            if (wizScript.hasAuthority)
//            {
//                TargetDisplays[0].SetTarget(wizScript);
//                allWizards[0] = wizScript;
//                wizards.RemoveAt(i);
//            }
//        }
//
//        for (int i = 0; i < wizards.Count; i++)
//        {
//            Wizard wizScript = wizards[i].GetComponent<Wizard>();
//
//            TargetDisplays[i + 1].SetTarget(wizScript);
//            allWizards[i + 1] = wizScript;
//        }

//        for (int i = 0; i < TargetDisplays.Length && i < wizards.Count; i++)
//        {
//            Wizard wizScript = wizards[i].GetComponent<Wizard>();
//            TargetDisplays[i].SetTarget(wizScript);
//            allWizards[i] = wizScript;
//
//            if (wizScript.hasAuthority)
//            {
//                spellStatusDisplay.wizardWithStatus = wizScript;
//            }
//        }

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
//                Debug.Log("Excuse you , id is: " + wizScript.WizardId);
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

//		Spell lockedSpell = allWizards[0].getLockedSpell();
//
//		if (lockedSpell != null)
//		{
//			playerConnectionObject.UseSpellOnBoss(lockedSpell);
//			allWizards[0].unlockSpell();
//		}
    }

    private void onWizardTargetSelected(int wizId)
    {
        Spell lockedSpell = allWizards[0].getLockedSpell();
//        Debug.Log("Spell that was locked > " + lockedSpell.ToString());

        if (lockedSpell != null && !allWizards[0].castCooldown)
        {
//            Debug.Log("spell checks out");
            if (lockedSpell.Type == Spell.TargetType.Party)
            {
                int[] wizIds = new int[allWizards.Length];

                for (var i = 0; i < allWizards.Length; i++)
                {
                    wizIds[i] = allWizards[i].WizardId;
                }

                playerConnectionObject.CmdWizardUseSpellOnWizards(wizIds, allWizards[0].WizardId);

//				playerConnectionObject.UseSpellOnWizards(allWizards, lockedSpell);
            }
            else if (lockedSpell.Type == Spell.TargetType.Single)
            {
                playerConnectionObject.CmdWizardUseSpellOnWizards(new int[] {wizId}, allWizards[0].WizardId);

//				foreach (Wizard wizard in allWizards)
//				{
//					if (wizard.WizardId == wizId)
//					{
//						playerConnectionObject.UseSpellOnWizards(new Wizard[] {wizard}, lockedSpell);
//						break;
//					}
//				}
            }

//            allWizards[0].unlockSpell();
        }
    }


//    ////////////////////
//    ////
//    //TODO: improve / optimize this code.
//    //TODO: improve the way listeners are added / wizard objects are found
//    [ClientRpc]
//    public void RpcUpdateTargetDisplayWizards()
//    {
//        UpdateTargetsDisplays();
//        Debug.Log("Updating displays");
//    }
}