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
    public bool isDead = false; // 좀비가 죽었는지 확인하는 플래그

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
        if (isDead) return; // 좀비가 이미 죽었으면 아무것도 하지 않음

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
        if (isDead) return;

        isDead = !isDead;

        dropItem.Item();
        DeactivateEnemy();
        experienceManager.AddExperience(5);
        ScoreManager.instance.AddPoint();
        StartCoroutine(ReleaseZombieAfterDelay(2f));
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

    public void DeactivateEnemy()
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
