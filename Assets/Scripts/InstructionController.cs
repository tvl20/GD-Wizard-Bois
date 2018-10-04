using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InstructionController : NetworkBehaviour
{
    public float timeLeft;

    [SyncVar] public bool roundActive = false;

    [SyncVar] public bool gameWon = false;
    [SyncVar] public bool gameLost = false;

    public bool sentReply = false;

    private int numbPlayers;

    private Dictionary<NetworkIdentity, TouchPatternInput.UniquePatterns> replies = new Dictionary<NetworkIdentity, TouchPatternInput.UniquePatterns>();

    private void Start()
    {
        if (!isServer) return;

        numbPlayers = NetworkManager.singleton.numPlayers;
    }

    private void Update()
    {
        if (roundActive)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
//                Debug.Log("RESETTING TIMER");

                if (!isServer) return;
                roundActive = false;

                gameWon = roundIsWon();
                gameLost = !gameWon;
            }
        }


        numbPlayers = NetworkManager.singleton.numPlayers;
    }

    public void addReply(NetworkIdentity playerID, TouchPatternInput.UniquePatterns patternReply)
    {
//        if (!roundActive || sentReply) return;
        if (!roundActive) return;

//        Debug.Log("from: " + playerID + " a thingy added to the replies: " + patternReply);
        replies.Add(playerID, patternReply);
        sentReply = true;
    }

    private bool roundIsWon()
    {
//        Debug.Log("checking victory condition");
        HashSet<TouchPatternInput.UniquePatterns> patterns = new HashSet<TouchPatternInput.UniquePatterns>();

        if (replies.Count != numbPlayers) return false;
//        Debug.Log("all players submitted something");

        foreach (KeyValuePair<NetworkIdentity,TouchPatternInput.UniquePatterns> pair in replies)
        {
            bool noDuplicate = patterns.Add(pair.Value);
            if (!noDuplicate)
            {
                return false;
            }
        }
//        Debug.Log("no duplicates found");

        return true;
    }

    [ClientRpc]
    public void RpcBeginNextRound()
    {
        roundActive = true;
        sentReply = false;
        timeLeft = 5;

        if (!isServer) return;
//        previousReplies = new Dictionary<NetworkIdentity, TouchPatternInput.UniquePatterns>(replies);
        replies.Clear();

    }
}
