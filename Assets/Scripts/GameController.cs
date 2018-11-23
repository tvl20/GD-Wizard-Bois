using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public AudioClip defeatSound;
    public AudioClip victorySound;
    public AudioSource winconditionSource;

    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;

    [SerializeField] private TargetingController targetingController;

    [SerializeField] private float endgameScreenDelay = 3;
    [SerializeField] private float reloadSceneDelay = 3;

    private Wizard[] allWizards;

    private void Start()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);

        if (!isServer) return;

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
        bool winConditionMet = !targetingController.boss.healthScript.isAlive;
        Debug.Log("win? " + winConditionMet);
        if (winConditionMet)
        {
            Debug.Log("game is won");
            RpcGameEnd(true);
            if (isServer) Invoke("reload", reloadSceneDelay + endgameScreenDelay);
        }

        Debug.Log("lose?");
        bool loseConditionMet = true;
        foreach (Wizard wizard in allWizards)
        {
            if (wizard.healthScript.isAlive)
            {
                loseConditionMet = false;
                break;
            }
        }

        if (loseConditionMet)
        {
            Debug.Log("game is lost");
            RpcGameEnd(false);
            if (isServer) Invoke("reload", reloadSceneDelay + endgameScreenDelay);
        }
    }

    private void reload()
    {
        NetworkManager.singleton.ServerChangeScene(NetworkManager.networkSceneName);
    }

    private IEnumerator showEndgameScreen(bool victory, float screenDelay, float reloadDelay)
    {
        yield return new WaitForSeconds(screenDelay);
        if (victory)
        {
            victoryScreen.SetActive(true);

            winconditionSource.clip = victorySound;
            winconditionSource.Play();
        }
        else
        {
            defeatScreen.SetActive(true);

            winconditionSource.clip = defeatSound;
            winconditionSource.Play();
        }
    }

    /////////////// RPC's
    [ClientRpc]
    private void RpcGameEnd(bool victory)
    {
        StartCoroutine(showEndgameScreen(victory, endgameScreenDelay, reloadSceneDelay));
    }
}