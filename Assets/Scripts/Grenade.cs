﻿using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float radius = 8f;
    public float power = 3000f;
    private void OnCollisionEnter(Collision other)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var nearby in colliders)
        {
            var rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, transform.position, radius, 1f);
            }
        }
        print(other.transform.name);
        Destroy(gameObject);
    }
}