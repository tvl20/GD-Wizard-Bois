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
	[SerializeField] private Text targetNameText;
	[SerializeField] private Slider healthSlider;

	private void Start ()
	{
		targetButton.onClick.AddListener(onButtonClick);
		targetButton.interactable = false;

		targetNameText.gameObject.SetActive(false);
	}

	public void SetTarget(Wizard target)
	{
		if (targetWizard != null)
		{
			targetWizard.transform.SetParent(null);

			targetWizard.healthScript.EventOnTakeDamage -= onHealthUpdate;
			targetWizard.healthScript.EventOnHealingReceived -= onHealthUpdate;
		}

		targetWizard = target;

		if (targetWizard == null)
		{
			targetButton.interactable = false;

			healthSlider.gameObject.SetActive(false);
		}
		else
		{
			targetWizard.transform.SetParent(this.transform);

			targetButton.transform.SetAsLastSibling();

			targetWizard.transform.localPosition = Vector3.zero;
			targetWizard.transform.localScale = Vector3.one;

			targetWizard.healthScript.EventOnTakeDamage += onHealthUpdate;
			targetWizard.healthScript.EventOnHealingReceived += onHealthUpdate;

			targetButton.interactable = true;

			if (targetWizard.hasAuthority)
			{
				targetNameText.text = "Self";
			}
			else
			{
				targetNameText.text = "Wizard " + targetWizard.WizardId;
			}

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
		float sliderValue = (float) targetWizard.healthScript.GetCurrentHealth() / targetWizard.healthScript.MaxHealth;
		healthSlider.value = sliderValue;

		if (sliderValue <= 0) onWizardDeath();
	}

	private void onWizardDeath()
	{
		targetButton.interactable = false;
	}
}
