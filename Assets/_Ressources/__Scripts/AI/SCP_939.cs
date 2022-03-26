using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class SCP_939 : MonoBehaviour
{
    
    [Header("Functional Options")]
    [SerializeField] private bool useSounds = true;

    [Header("Sounds")]
    [SerializeField] private int audioSourceType;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] lureSounds = default;
    [SerializeField] private AudioClip[] attackSounds = default;
    [SerializeField] private AudioClip[] alertSounds = default;
    
    [SerializeField] public Animator animator;

    private States _state = States.Idle;
    //private bool _stateSet = false;
    private float _timer = 0;
    private bool _playedSound = false;


    private enum States
    {
        Idle, Idle2 , Walk, Chase, Attack1, Attack2, Vocalize, // Hearing, Patrol
    }
    
     

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
    }
    
    
    private void UpdateNpc()
    {
        switch (_state)
        {
            case States.Idle:
            {
                //lookForPlayer();
                animator.Play("Idle 1");
                break;
            }
            case States.Idle2:
            {
               // lookForPlayer();
                animator.Play("Idle 2");
                break;
            }
            case States.Walk:
            {
                //lookForPlayer();
                animator.Play("Walk");
                break;
            }
            case States.Chase:
            {
                //chasePlayer();
                //audioSource.PlayOneShot(attackSounds[0]);
                animator.Play("Chase");
                break;
            }
            case States.Attack1:
            {
                Attack();
                break;
            }
            case States.Attack2:
            {
                Attack2();
               // audioSource.PlayOneShot(attackSounds[Random.Range(1, attackSounds.Length - 1)]);
                animator.Play("Attack 2");
                break;
            }
            case States.Vocalize:
            {
                Vocalize();
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Vocalize()
    {
        if (!_playedSound)
        {
           audioSource.PlayOneShot(lureSounds[Random.Range(0, lureSounds.Length)]);
           _playedSound = true;
        }
        animator.Play("Vocal");
    }
    

    private void Attack()
    {
        
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            audioSource.PlayOneShot(attackSounds[Random.Range(1, attackSounds.Length - 1)]);

        }
        animator.Play("Attack 1");
    }
    
    private void Attack2()
    {
        audioSource.PlayOneShot(attackSounds[Random.Range(1, attackSounds.Length - 1)]);
        animator.Play("Attack 2");
    }

    private void chasePlayer()
    {
        throw new NotImplementedException();
    }

    private void lookForPlayer()
    {
        throw new NotImplementedException();
    }

    private void StatesChanger()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _state = States.Vocalize;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _state = States.Chase;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _state = States.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            _state = States.Idle2;
        }

    }
    private void Update()
    {
        UpdateNpc();
        StatesChanger();
    }
}
