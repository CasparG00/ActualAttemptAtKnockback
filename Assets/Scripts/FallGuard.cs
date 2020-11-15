using UnityEngine;

public class FallGuard : MonoBehaviour
{

    public float fallLimit = -200;
    
    private Rigidbody _rb;
    private Transform _spawnPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!(transform.position.y <= fallLimit)) return;
        transform.position = _spawnPoint.position;
        _rb.velocity = Vector3.zero;
    }
}
