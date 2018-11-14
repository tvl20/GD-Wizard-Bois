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

    [SerializeField] private float endgameScreenDelay = 3;
    [SerializeField] private float reloadSceneDelay = 3;

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
        Debug.Log("updating playerlist");

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
                Debug.Log("added victory check for wiz: " + wizard.WizardId);
            }
        }
    }

    private void checkVictoryCondition()
    {
        Debug.Log("Checking Victory Condition");

        bool winConditionMet = !targetingController.boss.healthScript.isAlive;
        Debug.Log("win? " + winConditionMet);
        if (winConditionMet)
        {
            Debug.Log("game is won");
            StartCoroutine(showEndgameScreen(true, endgameScreenDelay, reloadSceneDelay));
        }

        Debug.Log("lose?");
        bool loseConditionMet = true;
        Debug.Log(allWizards);
        foreach (Wizard wizard in allWizards)
        {
            Debug.Log(wizard);
            Debug.Log(wizard.healthScript);
            Debug.Log(wizard.healthScript.isAlive);
            if (wizard.healthScript.isAlive)
            {
                loseConditionMet = false;
                Debug.Log("lose");
                break;
            }
        }

        Debug.Log("Lose> " + loseConditionMet);
        if (loseConditionMet)
        {
            Debug.Log("game is lost");
            StartCoroutine(showEndgameScreen(false, endgameScreenDelay, reloadSceneDelay));
        }
    }

    private IEnumerator showEndgameScreen(bool victory, float screenDelay, float reloadDelay)
    {
        yield return new WaitForSeconds(screenDelay);
        if (victory)
        {
            victoryScreen.SetActive(true);
        }
        else
        {
            defeatScreen.SetActive(true);
        }

        if (!isServer) yield break;

        yield return new WaitForSeconds(reloadDelay);
        NetworkManager.singleton.ServerChangeScene(NetworkManager.networkSceneName);
    }
}