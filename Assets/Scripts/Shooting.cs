using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 50;
    public float knockbackStrength;
    public float fireRate;
    public Transform cam;

    private Rigidbody _rb;

    private bool _canShoot = true;
    private bool _pressedShoot;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_canShoot || !Input.GetMouseButtonDown(0)) return;
        _rb.AddForce(-cam.forward * (knockbackStrength), ForceMode.Impulse);
        StartCoroutine(ShotDelay());
    }

    private IEnumerator ShotDelay()
    {
        _canShoot = false;
        
        yield return new WaitForSeconds(fireRate);
        
        _canShoot = true;
    }
}
