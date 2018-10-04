using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(InstructionController))]
public class InstructionControllerUI : NetworkBehaviour
{
    public Image statusImage;
    public Text timerText;
    public Button nextGameButton;

    private InstructionController instructionController;

    private void Start()
    {
        instructionController = GetComponent<InstructionController>();

        if (!isServer) nextGameButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        timerText.text = Math.Round(instructionController.timeLeft, 2).ToString();

        if (instructionController.roundActive)
        {
            if (instructionController.sentReply)
                statusImage.color = Color.yellow;
            else
                statusImage.color = Color.white;
        }
        else
        {
            if (instructionController.gameWon)
                statusImage.color = Color.green;
            else if (instructionController.gameLost)
                statusImage.color = Color.red;
        }
    }

    public void startNextMatch()
    {
        if (!isServer) return;
        instructionController.RpcBeginNextRound();
    }
}
