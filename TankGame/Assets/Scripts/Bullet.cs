using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject Bomb;
    private GameObject efx;
    private ParticleSystem _particleSystem;
    public LayerMask targerMask;
    private Collider[] colliders;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            efx = Instantiate(Bomb);
            _particleSystem = efx.GetComponent<ParticleSystem>();
            efx.transform.position = transform.position;
            _particleSystem.Play();
            
            gameObject.SetActive(false);
        }
    }
}
