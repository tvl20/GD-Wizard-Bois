using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DamageAble))]
public class BossEnemy : NetworkBehaviour
{
    public List<BossSpell> spells;

    public bool alive = true;
    public DamageAble healthScript;
//	public HealthStatus statusScript;

    private List<Wizard> allWizards;

    private void Start()
    {
        healthScript = GetComponent<DamageAble>();
        healthScript.EventOnZeroHealth += onZeroHealthHandler;

        if (isServer)
        {
            TimerController timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>();
            timer.onTimerTick.AddListener(onTimerTick);
            timer.onTimerStart.AddListener(onTimerStart);
        }
    }

    private void onTimerStart()
    {
        allWizards = new List<Wizard>();

        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizard");
        foreach (GameObject wizardObj in wizards)
        {
            Wizard wizardScript = wizardObj.GetComponent<Wizard>();
            allWizards.Add(wizardScript);
        }
    }

    private void onTimerTick()
    {
//        Debug.Log("Boss Using Boss Move");

        // TODO: add a smarter way of choosing spells
        int spellUsedIndex = Random.Range(0, spells.Count);
        BossSpell usedSpell = spells[spellUsedIndex];

        if (usedSpell.Type == BossSpell.TargetType.Party)
        {
            foreach (Wizard wizard in allWizards)
            {
                wizard.healthScript.TakeDamage(usedSpell.Damage);
            }
        }
        else if (usedSpell.Type == BossSpell.TargetType.Single)
        {
            int targetWizardId = Random.Range(0, allWizards.Count);
            Wizard targetWizard = allWizards[targetWizardId];
            targetWizard.healthScript.TakeDamage(usedSpell.Damage);
        }
        else if (usedSpell.Type == BossSpell.TargetType.Self)
        {
            healthScript.TakeDamage(usedSpell.Damage);
        }
    }

    private void onZeroHealthHandler()
    {
        alive = false;
    }
}