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
        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizard");

        allWizards = new Wizard[wizards.Length];

        for (int i = 0, j = 1; i < wizards.Length && j < TargetDisplays.Length; i++, j++)
        {
            Wizard wizScript = wizards[i].GetComponent<Wizard>();

            if (wizScript.hasAuthority)
            {
                TargetDisplays[0].SetTarget(wizScript);
                allWizards[0] = wizScript;
                j--;
            }
            else
            {
                TargetDisplays[j].SetTarget(wizScript);

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

        setTargetMarkerParent(bossTargetIndicaterPos);
    }

    private void onWizardTargetSelected(int wizId, Transform targetMarkerPos)
    {
        targetId = wizId;

        setTargetMarkerParent(targetMarkerPos);
    }

    private void setTargetMarkerParent(Transform parent)
    {
        targetMarker.transform.SetParent(parent);
        targetMarker.transform.localPosition = Vector3.zero;
        targetMarker.transform.localScale = Vector3.one;
    }
}