using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTestUI : MonoBehaviour
{
	public DamageTest healthScript;
	public Text healthRemaining;
	public Text healthChange;

	void Start ()
	{
		healthScript.EventOnTakeDamage += OnTakeUpdateUi;
		OnTakeUpdateUi(0);
	}

	private void OnTakeUpdateUi(int amount)
	{
		healthRemaining.text = healthScript.health.ToString();
		healthChange.text = amount.ToString();
	}
}
