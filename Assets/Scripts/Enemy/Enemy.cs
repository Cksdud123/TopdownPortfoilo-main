using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolAble
{
    public float HP = 100;
    public Animator animator;
    [HideInInspector] public RagdollManager ragdollManager;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public ExperienceManager experienceManager;

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
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 2.5f), Random.Range(0f, 0.25f));
        DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, damageAmount.ToString(), Color.yellow);

        Debug.Log("총알 맞음!! 현재 체력" + HP);
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
        experienceManager.AddExperience(5);
        DeactiveEnemy();
        StartCoroutine(ReleaseZombieAfterDelay(5f));
    }

    private IEnumerator ReleaseZombieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseObject();
    }

    public void DeactiveEnemy()
    {
        if (ragdollManager != null)
        {
            ragdollManager.setRigidbodyState(false);
        }
        navMeshAgent.enabled = false;
        animator.enabled = false;
    }
}
