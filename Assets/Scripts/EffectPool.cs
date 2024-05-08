using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : PoolAble
{
    //public static EffectPool instance;

    private void Start()
    {
        Invoke("DestroyEffect", 1f);
    }
    public void DestroyEffect()
    {
        ReleaseObject();
    }
}
