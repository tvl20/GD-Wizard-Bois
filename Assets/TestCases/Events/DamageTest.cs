using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageTest : NetworkBehaviour
{
	public int maxHealth = 10;
	public int health;

	public delegate void EventTakeDamageDelegate(int amount);

	[SyncEvent] public event EventTakeDamageDelegate EventOnTakeDamage;

	// Add event handler to the event
	private void Start()
	{
		health = maxHealth;
		EventOnTakeDamage += TakeTakeDamage;
	}

	// Call from outside
	public void damage(int amount)
	{
		CmdTakeDamage(amount);
	}

	// Call to the server to invoke event
	[Command]
	private void CmdTakeDamage(int amount)
	{
		if (EventOnTakeDamage != null)
			EventOnTakeDamage(amount);
	}

	// Event handler that applies the amount of damage
	private void TakeTakeDamage(int amount)
	{
		health -= amount;
	}
}
