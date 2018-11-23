using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class TimerController : NetworkBehaviour
{
	public GameObject startButton;

	public float maxTimeLimit;
	public float currentTimer;

	[SyncVar] public bool GameActive = false;
	public UnityEvent onTimerTick = new UnityEvent();
	public UnityEvent onTimerStart = new UnityEvent();

	private void Start()
	{
		currentTimer = maxTimeLimit;
		if (!isServer) startButton.SetActive(false);
	}

	private void Update()
	{
		if (GameActive)
		{
			currentTimer -= Time.deltaTime;
			if (currentTimer <= 0)
			{
				currentTimer = 0;

				if (!isServer) return;
				RpcResetTimer();
			}
		}
	}

	public void startTimer()
	{
		startButton.SetActive(false);

		if (isServer)
		{
			GameActive = true;
			RpcStartTimer();
		}
	}

	/////////////////// RPC's
	[ClientRpc]
	private void RpcResetTimer()
	{
		currentTimer = maxTimeLimit;
		onTimerTick.Invoke();
	}

	[ClientRpc]
	private void RpcStartTimer()
	{
		currentTimer = maxTimeLimit;
		onTimerStart.Invoke();
	}
}
