using UnityEngine;

public class FallGuard : MonoBehaviour
{

    public float fallLimit = -200;

    private Rigidbody _rb;
    private PlayerHealth _ph;
    private Transform _spawnPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ph = GetComponent<PlayerHealth>();

        _spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void Update()
    {
        if (!(transform.position.y <= fallLimit)) return;
        _ph.Damage(1);
        transform.position = _spawnPoint.position;
        _rb.velocity = Vector3.zero;

    }
}
