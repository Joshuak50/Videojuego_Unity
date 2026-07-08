using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movimiento_zombie : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum ZombieState { Idle, Chase, Attack, Dead }
    public Animator animator;
    public ZombieState currentState = ZombieState.Idle;
    public Transform player;

    public float ChaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;
    private CapsuleCollider capsuleCollider;

    private bool isAttacking;
    private float lastAttackTime; // Corregido a float

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown; // Permite que pueda atacar inmediatamente si está en rango
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case ZombieState.Idle:

                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", false);
                if (distanceToPlayer <= ChaseDistance)
                    currentState = ZombieState.Chase;
                break;

            case ZombieState.Chase:
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                navAgent.SetDestination(player.position);
                if (distanceToPlayer <= attackDistance)
                    currentState = ZombieState.Attack;
                break;

            case ZombieState.Attack:
                animator.SetBool("IsAttacking", true);
                navAgent.SetDestination(transform.position); // Detiene al zombi

                if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
                {
                    StartCoroutine(PerformAttack());
                }

                if (distanceToPlayer > attackDistance)
                    currentState = ZombieState.Chase;
                break;

            case ZombieState.Dead:
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsDead", true);
                navAgent.enabled = false;
                capsuleCollider.enabled = false;
                enabled = false;
                Debug.Log("Dead");
                break;
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        Movimiento_jugador movimiento_Jugador = player.GetComponent<Movimiento_jugador>();
        if (movimiento_Jugador!=null)
        {
            movimiento_Jugador.TakeDamage(damage);
        }
        Debug.Log("Zombie attacking...");

        yield return new WaitForSeconds(attackDelay); // Espera el tiempo de animación de ataque

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            Debug.Log("Player receives damage!");
            // Aquí puedes agregar la lógica de daño al jugador
        }

        lastAttackTime = Time.time;
        isAttacking = false;
    }
}
