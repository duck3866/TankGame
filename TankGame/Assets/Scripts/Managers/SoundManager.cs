using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instnace = null;

    public enum Sfx
    {
        Fire,
        Bomb,
        GameOver,
        Clear
    }
    private void Awake()
    {
        if (Instnace == null)
        {
            Instnace = this;
        }
    }

    public void PlaySound(Sfx sfx)
    {
        
    }
    
}
