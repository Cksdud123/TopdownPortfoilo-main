using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] float defaultBloomAngle = 3;
    [SerializeField] float walkBloomMultiplier = 1.5f;
    [SerializeField] float sprintBloomMultiplier = 2f;
    [SerializeField] float adsBloomMultiplier = 0.5f;

    MovementStateManager movement;
    ActionStateManager Action;

    float currentBloom;

    void Awake()
    {
        movement = GetComponentInParent<MovementStateManager>();
        Action = GetComponentInParent<ActionStateManager>();
    }

    public Vector3 BloomAngle(Transform barrelPos)
    {
        if (movement.currentState == movement.Idle) currentBloom = defaultBloomAngle;
        else if (movement.currentState == movement.Walk) currentBloom = defaultBloomAngle * walkBloomMultiplier;
        else if (movement.currentState == movement.Run) currentBloom = defaultBloomAngle * sprintBloomMultiplier;

        if (Action.currentState == Action.AimState) currentBloom *= adsBloomMultiplier;

        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotaion = new Vector3(randX, randY, randZ);
        return barrelPos.localEulerAngles + randomRotaion;
    }
}
