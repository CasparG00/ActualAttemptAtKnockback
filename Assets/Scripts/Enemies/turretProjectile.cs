using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float lifetime = 10f;
    
    private PlayerHealth _ph;

    private void Start()
    {
        _ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _ph.Damage(1);
        }

        Destroy(gameObject);
    }
}
