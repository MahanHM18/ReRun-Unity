using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private EnemyAnimation _animation;

    public float FollowDistance;
    public float AttackDistance;

    public Transform AttackPoint;
    public float AttackPointDistance;
    public LayerMask PlayerLayer;

    private bool _isDead;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindWithTag("Player").transform;
        _animation = GetComponent<EnemyAnimation>();
    }

    void Start()
    {
        _isDead = false;
    }

    void Update()
    {
        if (_isDead)
            return;
        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance < FollowDistance && distance > AttackDistance)
        {
            FollowTheTarget();
        }
        else if (distance < AttackDistance && distance < FollowDistance)
        {
            Attack();
        }
        else if (distance > AttackDistance && distance > FollowDistance)
        {
            Idle();
        }
    }

    private void FollowTheTarget()
    {
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("Following the player");
        _animation.Run(true);
        _animation.Attack(false);
        _navMeshAgent.speed = 3.5f;
        _navMeshAgent.isStopped = false;
    }

    private void Attack()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.speed = 0;
        _animation.Attack(true);
        //Debug.Log("enemy is attacking");
        Vector3 Distance = _target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Distance);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
    }

    private void Idle()
    {
        _navMeshAgent.speed = 0;
        _navMeshAgent.isStopped = true;
        //Debug.Log("enemy idle");
        _animation.Attack(false);
        _animation.Run(false);
    }

    public void AttackEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(AttackPoint.position, AttackPointDistance, PlayerLayer);

        foreach (Collider item in colliders)
        {
            Debug.Log($"Player name is : {item.gameObject.name}");
            item.GetComponent<Health>().TakeDamage(13);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackPointDistance);
    }

    public void Death()
    {
        _animation.Death();
        Debug.Log($"{gameObject.name} is dead ?! :|");
        _navMeshAgent.enabled = false;
        _isDead = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}