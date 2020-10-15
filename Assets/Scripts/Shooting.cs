using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate;
    public float bulletVelocity = 10;
    
    public Transform cam;
    public Transform barrel;
    public GameObject grenade;

    private bool _canShoot = true;
    private bool _pressedShoot;

    private void Update()
    {
        if (!_canShoot || !Input.GetMouseButtonDown(0)) return;
        var grenadeInstance = Instantiate(grenade, barrel.position, Quaternion.identity);
        var grenadeRb = grenadeInstance.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), grenadeRb.GetComponent<Collider>());
        grenadeRb.AddForce(cam.forward * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(ShotDelay());
    }

    private IEnumerator ShotDelay()
    {
        _canShoot = false;
        
        yield return new WaitForSeconds(fireRate);
        
        _canShoot = true;
    }
}
