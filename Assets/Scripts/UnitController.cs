using UnityEngine;
using UnityEngine.AI;

//attach to script that has navmeshagent
public class UnitController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float attackRange = 10f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;
    public float stopDistance = 1f;
    public LayerMask attackMask;

    private float currentHealth;
    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private float lastAttackTime;

    private void Awake()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Attack(Transform target)
    {
        if (Time.time > lastAttackTime + attackSpeed)
        {
            animator.SetTrigger("Attack");
            this.target = target;
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackRange)
            {
                animator.SetTrigger("Attack");
                target.GetComponent<UnitController>().TakeDamage(attackDamage);
                target = null;
            }
            else if (distance > agent.stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}