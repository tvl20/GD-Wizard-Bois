//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//[RequireComponent(typeof(Text))]
//public class SpellStatusDisplay : MonoBehaviour
//{
//	public Wizard wizardWithStatus;
//	private Text textField;
//
//	private void Start ()
//	{
//		textField = GetComponent<Text>();
//	}
//
//	private void Update()
//	{
//		if (wizardWithStatus != null)
//		{
//			if (wizardWithStatus.castCooldown)
//			{
//				textField.text = "On cooldown.";
//			}
//			else
//			{
//				Spell lockedSpell = wizardWithStatus.getLockedSpell();
//				if (lockedSpell == null)
//				{
//					textField.text = "Ready to cast!";
//				}
//				else
//				{
//					textField.text = lockedSpell.SpellName;
//				}
//			}
//		}
//	}
//}
