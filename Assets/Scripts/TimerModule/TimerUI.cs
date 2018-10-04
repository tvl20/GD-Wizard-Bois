using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TimerController))]
public class TimerUI : MonoBehaviour
{
	public Text timerText;
	private TimerController timer;

	void Start ()
	{
		timer = GetComponent<TimerController>();
	}
	
	void Update ()
	{
		timerText.text =  Math.Round(timer.currentTimer, 2).ToString();
	}
}
