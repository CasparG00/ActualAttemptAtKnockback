using UnityEngine;

public class FallGuard : MonoBehaviour
{

    public float fallLimit = -200;

    private Rigidbody _rb;
    private PlayerStats _ps;
    private Transform _spawnPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ps = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (transform.position.y <= fallLimit && !_ps.hasFallProtection)
        {
            //_rb.velocity = Vector3.zero;

            _ps.Damage(1);
            transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        else if (transform.position.y <= _ps.fallProtectionActivator && _ps.hasFallProtection)
        {
            _rb.velocity = Vector3.zero;

            _rb.AddForce(Vector3.up * _ps.fallProtectionForce, ForceMode.Impulse);
            _ps.hasFallProtection = false;
        }
    }
}
