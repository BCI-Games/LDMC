using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event UnityAction onMonsterHit;
    public void MonsterHit()
    {
        if(onMonsterHit != null)
        {
            onMonsterHit();
        }
    }

    public event UnityAction onCanCaptureMonster;
    public void CanCaptureMonster()
    {
        if(onCanCaptureMonster != null)
        {
            onCanCaptureMonster();
        }
    }

    public event UnityAction onCaptureMonster;
    public void CaptureMonster()
    {
        if(onCaptureMonster != null)
        {
            onCaptureMonster();
        }
    }



}
