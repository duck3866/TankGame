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
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Player"))
        {
            efx = Instantiate(Bomb);
            _particleSystem = efx.GetComponent<ParticleSystem>();
            efx.transform.position = transform.position;
            _particleSystem.Play();
            if (other.gameObject.CompareTag("Player"))
            {
                IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
                damagable.TakeDamage(1);
            }
            gameObject.SetActive(false);
        }
    }
    //
    // private void PerformAttack()
    // {
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, targerMask);
    //     Debug.Log("dsdsdsd");
    //     foreach (var hitCollider in colliders)
    //     {
    //         Debug.Log("dfggg");
    //         if (hitCollider != null && hitCollider.TryGetComponent<IDamagable>(out var target))
    //         {
    //             Debug.Log("123425");
    //             target.TakeDamage(1);
    //             Debug.Log($"공격 성공: {hitCollider.name}");
    //         }
    //     }
    // }
}
