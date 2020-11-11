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

    [Header("Weapon Knockback")] 
    public Transform weapon;
    public AnimationCurve weaponKnockbackAnimationCurve;
    public float weaponKnockbackAnimationStrength = 2;

    private bool _canShoot = true;
    private bool _pressedShoot;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ShootWeapon();
    }

    private void ShootWeapon()
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
        
        var localPos = weapon.localPosition;
        var targetPos = new Vector3(0, weaponKnockbackAnimationStrength * 0.2f, -weaponKnockbackAnimationStrength);
        StartCoroutine(WeaponKnockback(localPos, localPos + targetPos, 0.5f, weaponKnockbackAnimationCurve));
        StartCoroutine(ShotDelay());
    }

    private IEnumerator ShotDelay()
    {
        _canShoot = false;

        yield return new WaitForSeconds(fireRate);
        
        _canShoot = true;
    }
    
    private IEnumerator WeaponKnockback(Vector3 origin, Vector3 target, float duration, AnimationCurve curve)
    {
        var journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;
            
            var percent = Mathf.Clamp01(journey / duration);
            var curvePercent = curve.Evaluate(percent);
            weapon.localPosition = Vector3.LerpUnclamped(origin, target, curvePercent);
    
            yield return null;
        }
    }
}
