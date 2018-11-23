using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DamageAble))]
public class Wizard : NetworkBehaviour
{
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioSource playerSource;

    public List<Spell> SpellBook;

    [SyncVar] public int WizardId;
    [SyncVar] public bool castCooldown = false;

    public DamageAble healthScript;

    private int selectAttackSound;

    [SerializeField] private Animator _animator;
    [SerializeField] private NetworkAnimator _networkAnimator;

    private PlayerConnection playerObject;
    public void SetPlayerObject(PlayerConnection obj)
    {
        playerObject = obj;
    }

    private void Awake()
    {
        healthScript = GetComponent<DamageAble>();
        healthScript.EventOnZeroHealth += setDeathTrigger;
    }

    // Start is executed before authority is given to this gameobject
    public override void OnStartAuthority()
    {
        int wizId = int.Parse(GetComponent<NetworkIdentity>().netId.ToString());
        CmdSetWizardId(wizId);

        GameObject.FindGameObjectWithTag("Input").GetComponent<TouchPatternInput>().OnFinishedSpell
            .AddListener(castSpell);

        GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerController>().onTimerTick
            .AddListener(resetCooldown);
    }

    private void castSpell(Spell spell)
    {
        if (castCooldown || spell == null || !healthScript.isAlive) return;


        int spellIndex = -1;
        for (int i = 0; i < SpellBook.Count; i++)
        {
            if (SpellBook[i].Equals(spell))
            {
                spellIndex = i;
                break;
            }
        }

        if (spellIndex > -1)
        {
            _networkAnimator.SetTrigger("CastingSpell");

            playerObject.CastSpell(spellIndex);

            playerSource.clip = attackSound;
            playerSource.Play();
        }
    }

    private void setDeathTrigger()
    {

        _networkAnimator.SetTrigger("DeathTrigger");

        playerSource.clip = deathSound;
        playerSource.Play();

    }

    private void resetCooldown()
    {
        CmdResetCooldown();
    }

    ////////////////////// Commands
    [Command]
    private void CmdSetWizardId(int id)
    {
        this.WizardId = id;
    }

    [Command]
    private void CmdResetCooldown()
    {
        castCooldown = false;
    }
}