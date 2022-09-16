using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Run(bool value)
    {
        _anim.SetBool("IsRunning", value);
    }

    public void Attack(bool value)
    {
        _anim.SetBool("IsAttacking", value);
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
        
    }

}
