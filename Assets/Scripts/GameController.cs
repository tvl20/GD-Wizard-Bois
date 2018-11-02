using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;

    [SerializeField] private TargetingController targetingController;
    private Wizard[] allWizards;

    private void Awake()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);

        targetingController.onWizardListChanged.AddListener(updatePlayerList);
        targetingController.boss.healthScript.EventOnZeroHealth += checkVictoryCondition;
    }

    private void updatePlayerList()
    {
        if (allWizards != null)
        {
            foreach (Wizard wizard in allWizards)
            {
                wizard.healthScript.EventOnZeroHealth -= checkVictoryCondition;
            }
        }

        Wizard[] newAllWizardsArray = targetingController.GetAllWizards();
        allWizards = newAllWizardsArray;

        if (allWizards != null)
        {
            foreach (Wizard wizard in allWizards)
            {
                wizard.healthScript.EventOnZeroHealth += checkVictoryCondition;
            }
        }
    }

    private void checkVictoryCondition()
    {
        bool winConditionMet = false;
        bool loseConditionMet = true;

        winConditionMet = !targetingController.boss.healthScript.isAlive;

        foreach (Wizard wizard in allWizards)
        {
            if (wizard.healthScript.isAlive)
            {
                loseConditionMet = false;
                break;
            }
        }

        if (winConditionMet)
        {
            victoryScreen.SetActive(true);
        }
        else if (loseConditionMet)
        {
            defeatScreen.SetActive(true);
        }
        else
        {
            return;
        }

        if (isServer) Invoke("reloadScene", 3);
    }

    private void reloadScene()
    {
        Debug.Log("Reloading scene");
        NetworkManager.singleton.ServerChangeScene(NetworkManager.networkSceneName);
    }
}