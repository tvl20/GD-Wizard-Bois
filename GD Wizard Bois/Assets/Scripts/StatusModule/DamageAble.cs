using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class DamageAble : NetworkBehaviour
{
	// TODO EXPAND THIS AND MAKE IT BETTER
	public int MaxHealth = 1;

	// Events
	// Take Damage
	public delegate void EventTakeDamageDelegate(int amount);
	[SyncEvent] public event EventTakeDamageDelegate EventOnTakeDamage;

	// Healing Received
	public delegate void EventHealingReceivedDelegate(int amount);
	[SyncEvent] public event EventHealingReceivedDelegate EventOnHealingReceived;

	// Zero Health
	public delegate void EventZeroHealthDelegate();
	[SyncEvent] public event EventZeroHealthDelegate EventOnZeroHealth;


//	public UnityEvent OnZeroHealthEvent = new UnityEvent();
//	public UnityEvent OnDamageTakenEvent = new UnityEvent();
//	public UnityEvent OnHealingReceivedEvent = new UnityEvent();

	// So that the ZeroHealthEvent wont keep firing every frame
	private bool alive = true;

	private int currentHealth;

	public int GetCurrentHealth()
	{
		return currentHealth;
	}

	private void Awake()
	{
		currentHealth = MaxHealth;

		EventOnTakeDamage += damageCurrentHealth;
		EventOnHealingReceived += healCurrentHealth;
		EventOnZeroHealth += onZeroHealth;

//		OnDamageTakenEvent.Invoke();
//		if (EventOnTakeDamage != null)
//			EventOnTakeDamage.Invoke(0);
	}

	/// <summary>
	/// Use this method do reduce the health. If a negative parameter is given it will be seen as healing and restore health
	/// </summary>
	/// <param name="amount">amount of damade taken, if negative it will be seen as healing</param>
	public void TakeDamage(int amount)
	{
		if (!alive) return;
//		Debug.Log("CLIENT - taking damage: " + amount);
		CmdChangeHealth(amount);
	}

	private void damageCurrentHealth(int amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
		}
	}

	private void healCurrentHealth(int amount)
	{
		currentHealth += amount;
		if (currentHealth > MaxHealth)
		{
			currentHealth = MaxHealth;
		}
	}

	private void onZeroHealth()
	{
		alive = false;
	}

	////////////////////////////////////
	////
	[Command]
	private void CmdChangeHealth(int amount)
	{
//		currentHealth -= amount;
//		Debug.Log("SERVER - taking damage: " + amount);

		if (amount > 0)
		{
//			RpcTriggerDamageEvent();
			if (EventOnTakeDamage != null)
				EventOnTakeDamage(amount);

			if (currentHealth - amount <= 0)
			{
//				currentHealth = 0;

				if (alive)
				{
//					alive = false;

//					RpcTriggerDeathEvent();
					if (EventOnZeroHealth != null)
						EventOnZeroHealth();
				}
			}
		}
		else
		{
//			if (currentHealth > MaxHealth)
//			{
//				currentHealth = MaxHealth;
//			}

//			RpcTriggerHealingEvent();
			if (EventOnHealingReceived != null)
				EventOnHealingReceived(-amount);
		}
	}


	/////////////////////////////////////
	////
//	[ClientRpc]
//	private void RpcTriggerDamageEvent()
//	{
//		Debug.Log("RPC DAMAGE EVENT");
//		OnDamageTakenEvent.Invoke();
//	}
//
//	[ClientRpc]
//	private void RpcTriggerDeathEvent()
//	{
//		OnZeroHealthEvent.Invoke();
//	}
//
//	[ClientRpc]
//	private void RpcTriggerHealingEvent()
//	{
//		Debug.Log("RPC HEALING EVENT");
//		OnHealingReceivedEvent.Invoke();
//	}
}
