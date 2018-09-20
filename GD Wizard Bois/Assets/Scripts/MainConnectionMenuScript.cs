using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainConnectionMenuScript : MonoBehaviour
{
    public InputField inputField;

    public void HostGame()
    {
        Debug.Log("REEEEE");
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.singleton.networkAddress = inputField.text;
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartClient();
    }
}
