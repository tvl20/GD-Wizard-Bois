using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TargetingController : NetworkBehaviour
{
    public UnityEvent onWizardListChanged = new UnityEvent();
    public BossEnemy boss;
    public Transform bossTargetIndicaterPos;
    public WizardTargetDisplay[] TargetDisplays;

    public GameObject targetMarker;

    // The Wizard object of the localplayer will always be at position [0]
    private Wizard[] allWizards;
    public Wizard[] GetAllWizards()
    {
        return allWizards;
    }

    // TODO: FIND A BETTER WAY TO DO THIS
    // This is the ID of the target
    // this will be -1 if the boss is the selected target (default)
    private int targetId = -1;
    public int GetTargetId()
    {
        return targetId;
    }

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

        UpdateTargetsDisplays();

        // to fix position of the target marker to match the default targetId
        onBossTargetSelected();
    }

    public void NewWizardConnected()
    {
        UpdateTargetsDisplays();
    }

    public Wizard GetWizardById(int id)
    {
        foreach (Wizard wizard in allWizards)
        {
            if (wizard.WizardId == id) return wizard;
        }

        return null;
    }

    public int[] GetAllWizardIds()
    {
        int[] wizIds = new int[allWizards.Length];

        for (var i = 0; i < allWizards.Length; i++)
        {
            wizIds[i] = allWizards[i].WizardId;
        }

        return wizIds;
    }

    private void UpdateTargetsDisplays()
    {
//        SpellStatusDisplay spellStatusDisplay = GameObject.FindGameObjectWithTag("SpellStatus").GetComponent<SpellStatusDisplay>();
        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizard");

        allWizards = new Wizard[wizards.Length];

        Debug.Log("updating wizlist to length: " + allWizards.Length);

        for (int i = 0, j = 1; i < wizards.Length && j < TargetDisplays.Length; i++, j++)
        {
            Wizard wizScript = wizards[i].GetComponent<Wizard>();

            if (wizScript.hasAuthority)
            {
                TargetDisplays[0].SetTarget(wizScript);
                allWizards[0] = wizScript;
//                spellStatusDisplay.wizardWithStatus = allWizards[0];
                j--;
            }
            else
            {
                TargetDisplays[j].SetTarget(wizScript);

//                Debug.Log("J = " + j);
                if (j < allWizards.Length)
                {
                    allWizards[j] = wizScript;
                }
            }
        }

        onWizardListChanged.Invoke();
    }

    public void onBossTargetSelected()
    {
        targetId = -1;
//        Debug.Log("Boss is clicked, setting pos to boss");

        setTargetMarkerParent(bossTargetIndicaterPos);

//        if (allWizards[0].getLockedSpell() != null && !allWizards[0].castCooldown)
//        {
//            playerConnectionObject.CmdWizardUseSpellOnBoss(allWizards[0].WizardId);
//        }

        // TODO: PUT TARGET MARKER OVER BOSS
    }

    private void onWizardTargetSelected(int wizId, Transform targetMarkerPos)
    {
        targetId = wizId;
//        Debug.Log("Wizard is clicked, setting pos to wiz");

        setTargetMarkerParent(targetMarkerPos);

//        Spell lockedSpell = allWizards[0].getLockedSpell();
//
//        if (lockedSpell != null && !allWizards[0].castCooldown)
//        {
//            if (lockedSpell.Type == Spell.TargetType.Party)
//            {
//                int[] wizIds = new int[allWizards.Length];
//
//                for (var i = 0; i < allWizards.Length; i++)
//                {
//                    wizIds[i] = allWizards[i].WizardId;
//                }
//
//                playerConnectionObject.CmdWizardUseSpellOnWizards(wizIds, allWizards[0].WizardId);
//            }
//            else if (lockedSpell.Type == Spell.TargetType.Single)
//            {
//                playerConnectionObject.CmdWizardUseSpellOnWizards(new int[] {wizId}, allWizards[0].WizardId);
//            }
//        }

        // TODO: PUT TARGET MARKER OVER WIZARD
    }

    private void setTargetMarkerParent(Transform parent)
    {
//        Debug.Log("Setting Pos to: " + parent.position.x + " - " + parent.position.y);
        targetMarker.transform.SetParent(parent);
        targetMarker.transform.localPosition = Vector3.zero;
        targetMarker.transform.localScale = Vector3.one;
    }
}