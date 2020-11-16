using UnityEngine;

public class FallGuard : MonoBehaviour
{

    public float fallLimit = -200;
    
    private Rigidbody _rb;
    private PlayerHealth _ph;
    private Transform _spawnPoint;
    private PlayerStats _ps;
    
    private float _fallProtectionForce = 15;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ph = GetComponent<PlayerHealth>();
        _ps = GetComponent<PlayerStats>();
        
        _spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void Update()
    {
        if (!(transform.position.y <= fallLimit)) return;
        if (_ps.hasFallProtection && !(transform.position.y <= fallLimit))
        {
            _rb.AddForce(Vector3.up * _fallProtectionForce, ForceMode.Impulse);
            _ps.hasFallProtection = false;
        }
        else
        {
            _ph.Damage(1);
            transform.position = _spawnPoint.position;
            _rb.velocity = Vector3.zero;
        }
    }
}
