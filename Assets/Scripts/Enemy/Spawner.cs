using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public SpawnData(GameObject enemy, float spawnChance)
        {
            this.Enemy = enemy;
            this.SpawnChance = spawnChance;
        }
        public GameObject Enemy;
        [Range(0, 1)]
        public float SpawnChance; // ���� Ȯ�� (0���� 1 ������ ��)
    }

    [SerializeField] private List<SpawnData> _enemySpawns; // ������ ��(Enemy)�� �׿� ���� ���� Ȯ���� ���� ����Ʈ
    [SerializeField][Range(1, 100)] private float _timeBetweenSpawns; // �� ���� ���� (�� ����)

    private float _timeTowardsNextSpawn; // ���� �������� ���� �ð��� �����ϴ� ����
    private float _sumOfSpawnChances; // ��� �� ���� Ȯ���� ��
    public float SpawnRate { get => 1 / _timeBetweenSpawns; set => _timeBetweenSpawns = 1 / value; }

    private void Start()
    {
        // ��� �� ���� Ȯ���� ���� ����ϰ� ����ȭ�մϴ�.
        foreach (SpawnData spawn in _enemySpawns)
        {
            _sumOfSpawnChances += spawn.SpawnChance;
        }
        NormalizeSpawnRates(); // ���� Ȯ���� ����ȭ�մϴ�.
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �ð��� �����ϸ� �������� ���� �����մϴ�.
        if (_timeTowardsNextSpawn >= _timeBetweenSpawns)
        {
            RandomSpawnEnemy();
        }
        _timeTowardsNextSpawn += Time.deltaTime;
    }

    // �������� ���� �����ϴ� �޼���
    public void RandomSpawnEnemy()
    {
        _timeTowardsNextSpawn = 0; // ���� �������� ���� �ð� �ʱ�ȭ
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f); // 0���� 1 ������ ������ �� ����
        float lowerRange = 0;
        float upperRange = 0;

        // �� ���� ���� Ȯ���� ���� �������� ����
        foreach (SpawnData spawn in _enemySpawns)
        {
            upperRange += spawn.SpawnChance;
            if (randomNumber >= lowerRange && randomNumber <= upperRange)
            {
                SpawnEnemy(spawn.Enemy); // �� ����
                return;
            }
            lowerRange = upperRange;
        }
    }

    // ���� ������ �����ϴ� �޼���
    public void SpawnEnemy(GameObject enemy)
    {
        Debug.Log("Spawning enemy: " + enemy.name);
        var zombieGo = ObjectPoolingManager.instance.GetGo(enemy.name);

        zombieGo.SetActive(true);

        Enemy enemyGo = zombieGo.GetComponent<Enemy>();

        enemyGo.transform.position = transform.position;
        enemyGo.transform.rotation = Quaternion.identity;

        enemyGo.ragdollManager.setRigidbodyState(true);
        enemyGo.ragdollManager.setColliderState(false);
        enemyGo.ragdollManager.ParentCollider.enabled = true;

        enemyGo.HP = 100;
        enemyGo.navMeshAgent.enabled = true;
        enemyGo.animator.enabled = true;
    }

    private void NormalizeSpawnRates()
    {
        if (_sumOfSpawnChances == 0)
        {
            return;
        }
        foreach (SpawnData spawn in _enemySpawns)
        {
            spawn.SpawnChance /= _sumOfSpawnChances;
        }
    }
}
