using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TargetWizardEvent : UnityEvent<int> { }

public class WizardTargetDisplay : MonoBehaviour
{
	public TargetWizardEvent OnWizardTargetClicked = new TargetWizardEvent();

	private Wizard targetWizard = null;

	[SerializeField] private Button targetButton;
	[SerializeField] private Text targetButtonText;
	[SerializeField] private Slider healthSlider;

	private void Start ()
	{
		targetButton.onClick.AddListener(onButtonClick);
		targetButton.interactable = false;

		targetButtonText.gameObject.SetActive(false);
		healthSlider.gameObject.SetActive(false);
	}

	public void SetTarget(Wizard target)
	{
//		if (targetWizard != null)
//		{
//			targetWizard.healthScript.OnDamageTakenEvent.RemoveListener(onHealthUpdate);
//			targetWizard.healthScript.OnHealingReceivedEvent.RemoveListener(onHealthUpdate);
////			Debug.Log(targetWizard.WizardId + " < wizid 1");
//		}

		targetWizard = target;
//		Debug.Log(targetWizard.WizardId + " < wizid 2");
//		Debug.Log("Target > " + targetWizard);
//		Debug.Log("Healthscript > " + targetWizard.healthScript);
//		Debug.Log("DamageEvent > "  + targetWizard.healthScript.OnDamageTakenEvent);

		if (targetWizard == null)
		{
			targetButton.interactable = false;

			targetButtonText.gameObject.SetActive(false);
			targetButtonText.text = "";

			healthSlider.gameObject.SetActive(false);
		}
		else
		{
//			targetWizard.healthScript.OnDamageTakenEvent.AddListener(onHealthUpdate);
//			targetWizard.healthScript.OnHealingReceivedEvent.AddListener(onHealthUpdate);
			targetWizard.healthScript.EventOnTakeDamage += onHealthUpdate;
			targetWizard.healthScript.EventOnHealingReceived += onHealthUpdate;

			targetButton.interactable = true;

			targetButtonText.gameObject.SetActive(true);
			if (targetWizard.hasAuthority)
			{
				targetButtonText.text = "Self";
			}
			else
			{
//				Debug.Log(targetWizard.WizardId + " < wizid");
				targetButtonText.text = "Wizard " + targetWizard.WizardId;
			}

			healthSlider.gameObject.SetActive(true);
			onHealthUpdate(0);
		}
	}

	public void onButtonClick()
	{
		if (targetWizard == null) return;
		OnWizardTargetClicked.Invoke(targetWizard.WizardId);
	}

	private void onHealthUpdate(int change)
	{
		Debug.Log("Health Update");
		float sliderValue = (float) targetWizard.healthScript.GetCurrentHealth() / targetWizard.healthScript.MaxHealth;
//		Debug.Log(sliderValue);
		healthSlider.value = sliderValue;
	}
}
