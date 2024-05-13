using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolable : PoolAble
{
    private void Start()
    {
        Invoke("DestroyAmmo", Random.Range(3.0f, 10.0f));
    }
    public void DestroyAmmo()
    {
        ReleaseObject();
    }
}