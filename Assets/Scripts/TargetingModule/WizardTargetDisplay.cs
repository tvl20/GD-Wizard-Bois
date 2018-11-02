using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TargetWizardEvent : UnityEvent<int, Transform> { }

public class WizardTargetDisplay : MonoBehaviour
{
	public TargetWizardEvent OnWizardTargetClicked = new TargetWizardEvent();

	public Transform targetIndicatorPosition;

	private Wizard targetWizard = null;

	[SerializeField] private Button targetButton;
//	[SerializeField] private Text targetButtonText;
	[SerializeField] private Text targetNameText;
	[SerializeField] private Slider healthSlider;

	private void Start ()
	{
		targetButton.onClick.AddListener(onButtonClick);
		targetButton.interactable = false;
		Debug.Log("added the onbuttonclick to :" + targetButton);

//		targetButtonText.gameObject.SetActive(false);
//		healthSlider.gameObject.SetActive(false);
		targetNameText.gameObject.SetActive(false);
	}

	public void SetTarget(Wizard target)
	{
		if (targetWizard != null)
		{
			targetWizard.transform.parent.SetParent(null);

			targetWizard.healthScript.EventOnTakeDamage -= onHealthUpdate;
			targetWizard.healthScript.EventOnHealingReceived -= onHealthUpdate;
		}

		targetWizard = target;

		if (targetWizard == null)
		{
			targetButton.interactable = false;

//			targetButtonText.gameObject.SetActive(false);
//			targetButtonText.text = "";

			healthSlider.gameObject.SetActive(false);
		}
		else
		{
			targetWizard.transform.parent.SetParent(this.transform);

			// this makes sure that the button is always the last int he list
			targetButton.transform.SetAsLastSibling();

			targetWizard.transform.parent.localPosition = Vector3.zero;
			targetWizard.transform.parent.localScale = Vector3.one;

			targetWizard.healthScript.EventOnTakeDamage += onHealthUpdate;
			targetWizard.healthScript.EventOnHealingReceived += onHealthUpdate;

			targetButton.interactable = true;

//			targetButtonText.gameObject.SetActive(true);
			if (targetWizard.hasAuthority)
			{
//				targetButtonText.text = "Self";
				targetNameText.text = "Self";
			}
			else
			{
//				targetButtonText.text = "Wizard " + targetWizard.WizardId;
				targetNameText.text = "Wizard " + targetWizard.WizardId;
			}

//			healthSlider.gameObject.SetActive(true);
			targetNameText.gameObject.SetActive(true);
			onHealthUpdate(0);
		}
	}

	public void onButtonClick()
	{
		if (targetWizard == null) return;
		OnWizardTargetClicked.Invoke(targetWizard.WizardId, targetIndicatorPosition);
	}

	private void onHealthUpdate(int change)
	{
//		Debug.Log("Health Update");
		float sliderValue = (float) targetWizard.healthScript.GetCurrentHealth() / targetWizard.healthScript.MaxHealth;
		healthSlider.value = sliderValue;

		if (sliderValue <= 0) onWizardDeath();
	}

	private void onWizardDeath()
	{
		targetButton.interactable = false;
	}
}
