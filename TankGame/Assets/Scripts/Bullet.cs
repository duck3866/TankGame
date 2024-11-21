using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private GameObject _efx;
    private ParticleSystem _particleSystem;
    public LayerMask targerMask;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,0.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Enemy"))
        {
            _efx = Instantiate(bomb);
            _particleSystem = _efx.GetComponent<ParticleSystem>();
            _efx.transform.position = transform.position;
            _particleSystem.Play();
            PerformAttack();
            gameObject.SetActive(false);
        }
    }
    
    private void PerformAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f,targerMask);
        foreach (var hitCollider in colliders)
        {
            if (hitCollider != null && hitCollider.TryGetComponent<IDamagable>(out var target))
            {
                target.TakeDamage(1);
                Debug.Log($"공격 성공: {hitCollider.name}");
            }
        }
    }
}
