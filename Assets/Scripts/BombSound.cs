using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSound : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _audioSource;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.Play();
    }
}
