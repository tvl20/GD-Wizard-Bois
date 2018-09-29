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
		HealthSource.OnDamageTakenEvent.AddListener(updateUI);
		HealthSource.OnHealingReceivedEvent.AddListener(updateUI);
		updateUI();
	}

	private void updateUI()
	{
		HealthBar.value = (float) HealthSource.GetCurrentHealth() / HealthSource.MaxHealth;
	}
}
