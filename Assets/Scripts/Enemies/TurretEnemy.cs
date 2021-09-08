using System.Collections;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public State state = State.Game;

    [HideInInspector] public MenuState menuState = MenuState.Active;
    
    public Transform turret;
    private Transform _tf;

    private Vector3 _shootDir;

    [Header("Detection Settings")] 
    public float maxDetectionRange = 100;
    private Transform _player;
    private bool _detected;

    [Header("Shooting Settings")] 
    public GameObject projectile;

    public float projectileSpeed = 5;
    public float fireRate = 5;
    private bool _canShoot;
    
    [Header("Weapon Knockback Animation")] 
    public float weaponKnockbackAnimationStrength = 2;
    [Space]
    public AnimationCurve weaponKnockbackAnimationCurve;

    private void Start()
    {
        _tf = transform;
        switch (state)
        {
            case State.Game:
                _player = GameObject.FindGameObjectWithTag("Player").transform;
                StartCoroutine(ShotDelay(fireRate));
                break;
            case State.Menu:
                StartCoroutine(ShotDelay(menuFireRate));
                _menuCamera = Camera.main;
                break;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Game:
                DetectPlayer();

                if (!_detected) return;
                UpdateRotation();

                if (_canShoot)
                {
                    Shoot();
                }

                break;
            case State.Menu:
                switch (menuState)
                {
                    case MenuState.Active:
                        Animate();
                        MenuShoot();
                        break;
                    case MenuState.Inactive:
                        break;
                }

                break;
        }
    }

    //Check if Player is within Range and Visible to the turret
    private void DetectPlayer()
    {
        var pos = _tf.position;
        _shootDir = (_player.position - pos).normalized;
        var hitDistance = 0f;

        if (Vector3.Distance(pos, _player.position) <= maxDetectionRange)
        {
            if (Physics.Raycast(pos, _shootDir, out var hit, maxDetectionRange))
            {
                _detected = hit.transform.CompareTag("Player");
                hitDistance = hit.distance;
            }
        }
        else
        {
            _detected = false;
        }
        
        if (_detected)
        {
            Debug.DrawRay(pos, _shootDir * hitDistance, Color.magenta);
        }
    }

    private void Shoot()
    {
        //Start Shooting when the Player is in View
        var instance = Instantiate(projectile, _tf.position, Quaternion.identity);

        instance.GetComponent<Rigidbody>().AddForce(_shootDir * projectileSpeed, ForceMode.Impulse);

        StartCoroutine(ShotDelay(fireRate));
        var localPos = turret.localPosition;
        var targetPos = new Vector3(0, weaponKnockbackAnimationStrength * 0.2f, -weaponKnockbackAnimationStrength);
        StartCoroutine(WeaponKnockback(localPos, localPos + targetPos, 0.5f, weaponKnockbackAnimationCurve));
    }

    //Limit Fire rate
    private IEnumerator ShotDelay(float rate)
    {
        _canShoot = false;

        yield return new WaitForSeconds(rate);

        _canShoot = true;
    }
    
    //Update turret rotation when it is able to shoot
    private void UpdateRotation()
    {
        var playerPos = _player.position;

        var relativePos = playerPos - transform.position;
        var toRotation = Quaternion.LookRotation(relativePos);
        _tf.rotation = Quaternion.Lerp( transform.rotation, toRotation, 10 * Time.deltaTime);
        
        var angles = _tf.eulerAngles;
        _tf.eulerAngles = new Vector3(0, angles.y, 0);
        
        turret.LookAt(playerPos, Vector3.up);
    }
    
    private IEnumerator WeaponKnockback(Vector3 origin, Vector3 target, float duration, AnimationCurve curve)
    {
        var journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;

            var percent = Mathf.Clamp01(journey / duration);
            var curvePercent = curve.Evaluate(percent);
            turret.localPosition = Vector3.LerpUnclamped(origin, target, curvePercent);

            yield return null;
        }
    }

    #region Only for the Main Menu

    [Header("Menu Settings")]
    public GameObject menuProjectile;
    public float menuFireRate = 1f;
    
    private Camera _menuCamera;
    private Vector3 _menuTarget;
    
    private void Animate()
    {
        var ray = _menuCamera.ScreenPointToRay(Input.mousePosition);
        var midPoint = (_tf.position - _menuCamera.transform.position).magnitude * 0.5f;

        _menuTarget = ray.origin + ray.direction * midPoint;

        _tf.LookAt(_menuTarget, Vector3.up);
        var angles = _tf.eulerAngles;
        _tf.eulerAngles = new Vector3(0, angles.y, 0);

        turret.LookAt(_menuTarget, Vector3.up);
    }

    private void MenuShoot()
    {
        if (!Input.GetMouseButtonDown(0) || !_canShoot) return;
        
        var position = _tf.position;
        var instance = Instantiate(menuProjectile, position, Quaternion.identity);

        var dir = (_menuTarget - position).normalized;

        instance.GetComponent<Rigidbody>().AddForce(dir * projectileSpeed, ForceMode.Impulse);

        StartCoroutine(ShotDelay(menuFireRate));
        
        var localPos = turret.localPosition;
        var targetPos = new Vector3(0, weaponKnockbackAnimationStrength * 0.2f, -weaponKnockbackAnimationStrength);
        StartCoroutine(WeaponKnockback(localPos, localPos + targetPos, 0.5f, weaponKnockbackAnimationCurve));
    }

    #endregion

    public enum State
    {
        Game,
        Menu
    }

    public enum MenuState
    {
        Active,
        Inactive
    }
}
