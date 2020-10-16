using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate = 1;
    public float bulletVelocity = 10;
    public float knockbackStrength = 10;

    public float pelletCount = 12;
    
    public Transform cam;
    public Transform barrel;
    public GameObject pellet;

    private bool _canShoot = true;
    private bool _pressedShoot;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_canShoot || !Input.GetMouseButtonDown(0)) return;
        
        _rb.AddForce(-cam.forward * knockbackStrength, ForceMode.Impulse);

        for (var i = 0; i < pelletCount; i++) {
            var instance = Instantiate(pellet, barrel.position, Quaternion.identity);
            var instanceRb = instance.GetComponent<Rigidbody>();
            
            var direction = cam.forward;
            var spread = Vector3.zero;
            spread += cam.up * Random.Range(-1f, 1f);
            spread += cam.right * Random.Range(-1f, 1f);
            
            direction += spread.normalized * Random.Range(0f, 0.2f);
            instanceRb.AddForce(direction * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(ShotDelay());
    }

    private IEnumerator ShotDelay()
    {
        _canShoot = false;
        
        yield return new WaitForSeconds(fireRate);
        
        _canShoot = true;
    }
}
