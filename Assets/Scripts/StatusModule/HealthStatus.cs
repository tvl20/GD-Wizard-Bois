using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DamageAble))]
public class HealthStatus : NetworkBehaviour
{
	public List<StatusEffect> ActiveEffects = new List<StatusEffect>();

	private void Start ()
	{
		GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>().onTimerTick.AddListener(onTimerTick);
	}

	public void ApplyStatusEffects(StatusEffect[] effects)
	{
		CmdAddStatusEffects(effects);
	}

	private void onTimerTick()
	{
		for (var i = 0; i < ActiveEffects.Count; i++)
		{
			ActiveEffects[i].TurnTickCount--;
			if (ActiveEffects[i].TurnTickCount <= 0)
			{
				ActiveEffects.RemoveAt(i);
				i--;
			}
		}
	}

	//////////////////////////////
	////
	[Command]
	private void CmdAddStatusEffects(StatusEffect[] effects)
	{
		RpcAddStatusEffects(effects);
	}

	//////////////////////////////
	////
	[ClientRpc]
	private void RpcAddStatusEffects(StatusEffect[] effects)
	{
		ActiveEffects.AddRange(effects);
	}
}
