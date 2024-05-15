using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : PoolAble
{
    public float HP = 100;
    public Animator animator;
    [HideInInspector] public RagdollManager ragdollManager;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public ExperienceManager experienceManager;

    public DropItem dropItem;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdollManager = GetComponent<RagdollManager>();
        experienceManager = FindObjectOfType<ExperienceManager>();
    }
    private void Start()
    {
        navMeshAgent.enabled = true;
        animator.enabled = true;
    }
    public void TakeDamage(float damageAmount)
    {
        HP -= damageAmount;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 1.5f), Random.Range(0f, 0.25f));
        DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, damageAmount.ToString(), Color.red);

        if (HP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    private void Die()
    {
        if (HP <= 0)
        {
            dropItem.Item();
            experienceManager.AddExperience(5);
            DeactiveEnemy();
            StartCoroutine(ReleaseZombieAfterDelay(2f));
        }
    }

    private IEnumerator ReleaseZombieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
        }

        ReleaseObject();
    }

    public void DeactiveEnemy()
    {
        if (ragdollManager != null)
        {
            ragdollManager.setRigidbodyState(false);
            ragdollManager.setColliderState(true);
            ragdollManager.ParentCollider.enabled = false;
        }

        navMeshAgent.enabled = false;
        animator.enabled = false;
    }
}
