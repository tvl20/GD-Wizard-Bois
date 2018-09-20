using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
	public TouchPatternInput touchPatternInput;

	private NetworkIdentity networkIdentity;

	private InstructionController instructionController;

	private void Start ()
	{
//		if (!isLocalPlayer) return;
		if (!isLocalPlayer) this.gameObject.SetActive(false);

		networkIdentity = GetComponent<NetworkIdentity>();

		touchPatternInput = GameObject.FindGameObjectWithTag("GameController").GetComponent<TouchPatternInput>();
		touchPatternInput.onFinishedPattern.AddListener(filterNonePatterns);

		instructionController = GameObject.FindGameObjectWithTag("InstructionController")
			.GetComponent<InstructionController>();
	}

	private void filterNonePatterns(TouchPatternInput.UniquePatterns pattern)
	{
		if (!isLocalPlayer) return;
		if (pattern != TouchPatternInput.UniquePatterns.None)
		{
			Debug.Log("SENDING PATTERN TO THE SERVER: " + pattern);
			CmdInputPattern(networkIdentity, pattern);
		}
	}

	////////////
	//// Commands
	[Command]
	private void CmdInputPattern(NetworkIdentity id, TouchPatternInput.UniquePatterns pattern)
	{
		Debug.Log("REPLY RECEIVED adding to > " + instructionController.ToString());
		instructionController.addReply(id, pattern);
	}
}
