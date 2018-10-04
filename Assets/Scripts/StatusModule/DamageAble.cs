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

	public bool isAlive = true;

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
	}

	/// <summary>
	/// Use this method do reduce the health. If a negative parameter is given it will be seen as healing and restore health
	/// </summary>
	/// <param name="amount">amount of damade taken, if negative it will be seen as healing</param>
	public void TakeDamage(int amount)
	{
		if (!isAlive) return;
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
		isAlive = false;
	}

	////////////////////////////////////
	////
	[Command]
	private void CmdChangeHealth(int amount)
	{
		if (amount > 0)
		{
			if (EventOnTakeDamage != null)
			{
				EventOnTakeDamage(amount);
			}

			if (currentHealth - amount <= 0)
			{
				if (isAlive)
				{
					if (EventOnZeroHealth != null)
					{
						EventOnZeroHealth();
					}
				}
			}
		}
		else
		{
			if (EventOnHealingReceived != null)
			{
				EventOnHealingReceived(-amount);
			}
		}
	}
}
