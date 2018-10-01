using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdater : MonoBehaviour
{
	public DamageAble HealthSource;
	public Slider HealthBar;

	void Start ()
	{
		HealthSource.EventOnTakeDamage += updateUI;
		HealthSource.EventOnHealingReceived += updateUI;

		updateUI(0);
	}

	private void updateUI(int change)
	{
		HealthBar.value = (float) HealthSource.GetCurrentHealth() / HealthSource.MaxHealth;
	}
}
