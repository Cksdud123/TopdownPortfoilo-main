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
        public float SpawnChance; // 스폰 확률 (0부터 1 사이의 값)
    }

    [SerializeField] private List<SpawnData> _enemySpawns; // 스폰할 적(Enemy)과 그에 대한 스폰 확률을 담은 리스트
    [SerializeField][Range(1, 100)] private float _timeBetweenSpawns; // 적 스폰 간격 (초 단위)

    private float _timeTowardsNextSpawn; // 다음 스폰까지 남은 시간을 추적하는 변수
    private float _sumOfSpawnChances; // 모든 적 스폰 확률의 합
    public float SpawnRate { get => 1 / _timeBetweenSpawns; set => _timeBetweenSpawns = 1 / value; }

    private void Start()
    {
        // 모든 적 스폰 확률의 합을 계산하고 정규화합니다.
        foreach (SpawnData spawn in _enemySpawns)
        {
            _sumOfSpawnChances += spawn.SpawnChance;
        }
        NormalizeSpawnRates(); // 스폰 확률을 정규화합니다.
    }

    // Update is called once per frame
    void Update()
    {
        // 다음 스폰 시간에 도달하면 무작위로 적을 스폰합니다.
        if (_timeTowardsNextSpawn >= _timeBetweenSpawns)
        {
            RandomSpawnEnemy();
        }
        _timeTowardsNextSpawn += Time.deltaTime;
    }

    // 무작위로 적을 스폰하는 메서드
    public void RandomSpawnEnemy()
    {
        _timeTowardsNextSpawn = 0; // 다음 스폰까지 남은 시간 초기화
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f); // 0부터 1 사이의 무작위 수 생성
        float lowerRange = 0;
        float upperRange = 0;

        // 각 적의 스폰 확률에 따라 무작위로 스폰
        foreach (SpawnData spawn in _enemySpawns)
        {
            upperRange += spawn.SpawnChance;
            if (randomNumber >= lowerRange && randomNumber <= upperRange)
            {
                SpawnEnemy(spawn.Enemy); // 적 스폰
                return;
            }
            lowerRange = upperRange;
        }
    }

    // 적을 실제로 스폰하는 메서드
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
