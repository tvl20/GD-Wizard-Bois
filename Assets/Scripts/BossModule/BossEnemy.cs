using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DamageAble))]
public class BossEnemy : NetworkBehaviour
{
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioSource bossSource;

    public List<BossSpell> spells;

    public DamageAble healthScript;
    //	public HealthStatus statusScript;

    private int selectAttackSound;

    private List<Wizard> allWizards;

    [SerializeField] private Animator _animator;

    private void Start()
    {
        healthScript = GetComponent<DamageAble>();

        if (isServer)
        {
            healthScript.EventOnZeroHealth += setDeathTrigger;

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

        _animator.SetTrigger("CastSpellTrigger");

        bossSource.clip = attackSound;
        bossSource.Play();

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

    private void setDeathTrigger()
    {
        _animator.SetTrigger("DeathTrigger");

        bossSource.clip = deathSound;
        bossSource.Play();
    }
}