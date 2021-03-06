﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("Base Shooting Settings")]
    public float knockbackStrength = 10;
    public float fireRate = 1;

    [HideInInspector] 
    public float currentFireRate;

    [Space]
    public Transform cam;
    public LayerMask layers;
    public EnemySpawningManager esm;

    private Rigidbody _rb;
    private PlayerStats _ps;
    private float _timer;
    private bool _canShoot = true;

    [Header("Pellet Settings")]
    public float pelletCount = 12;
    public float pelletSpread = 0.05f;

    public float maxRange = 50;

    [Header("UI Settings")]
    public GameObject cooldownBar;

    public float fadeTime = 0.2f;

    private Slider _coolDownBarSlider;
    private CanvasGroup _cooldownBarGroup;

    [Header("Weapon Animations")] 
    public float weaponKnockbackAnimationStrength = 2;
    [Space]
    public Transform weapon;
    public AnimationCurve weaponKnockbackAnimationCurve;
    
    public ShakeTransform st;
    public CameraShakeEvent data;

    public ParticleSystem shotParticles;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ps = GetComponent<PlayerStats>();

        _coolDownBarSlider = cooldownBar.GetComponent<Slider>();
        _cooldownBarGroup = cooldownBar.GetComponent<CanvasGroup>();
        _cooldownBarGroup.alpha = 0;

        currentFireRate = fireRate;
    }

    private void Update()
    {
        if (PlayerStats.IsDead || Pause.IsPaused) return;
        ShootWeapon();
        UpdateUI();
        ShotDelay();
    }

    private void ShootWeapon()
    {
        if (!_canShoot || !Input.GetMouseButtonDown(0)) return;
        
        _rb.AddForce(-cam.forward * knockbackStrength, ForceMode.Impulse);
        StartCoroutine(FadeUI(_cooldownBarGroup.alpha, _canShoot ? 1 : 0));
        _canShoot = false;

        for (var i = 0; i < pelletCount; i++)
        {
            var direction = cam.forward;
            var spread = Vector3.zero;
            
            spread += cam.up * Random.Range(-1f, 1f);
            spread += cam.right * Random.Range(-1f, 1f);

            direction += spread.normalized * Random.Range(0f, pelletSpread);

            if (!Physics.Raycast(transform.position, direction, out var hit, maxRange, layers)) continue;
            var hitTag = hit.transform.tag;

            switch (hitTag)
            {
                case "Enemy":
                    Destroy(hit.transform.gameObject);
                    esm.FreeSpawner(hit);
                    PlayerStats.Score += GameStats.TurretPoints;
                    break;

                case "Projectile":
                    Destroy(hit.transform.gameObject);
                    PlayerStats.Score += GameStats.ProjectilePoints;
                    break;

                case "Generator":
                    var lg = hit.transform.GetComponent<LaserGenerator>();
                    if (lg.isOn)
                    {
                        PlayerStats.Score += GameStats.GeneratorPoints;
                        lg.StartCoroutine(lg.Reset());
                    }

                    break;
            }

            if (hitTag == "Ground") continue;
            break;
        }

        var localPos = weapon.localPosition;
        var targetPos = new Vector3(0, weaponKnockbackAnimationStrength * 0.2f, -weaponKnockbackAnimationStrength);
        
        st.AddShakeEvent(data);
        shotParticles.Play();
        StartCoroutine(WeaponKnockback(localPos, localPos + targetPos, 0.5f, weaponKnockbackAnimationCurve));
    }

    private void ShotDelay()
    {
        if (!_canShoot)
        {
            _timer += Time.deltaTime;
        }

        if (!(_timer >= currentFireRate)) return;
        StartCoroutine(FadeUI(_cooldownBarGroup.alpha, _canShoot ? 1 : 0));
        _canShoot = true;
        _timer = 0;
    }

    private void UpdateUI()
    {
        _coolDownBarSlider.value = (currentFireRate - _timer) / currentFireRate;
    }

    private IEnumerator FadeUI(float start, float end)
    {
        var counter = 0f;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            _cooldownBarGroup.alpha = Mathf.Lerp(start, end, counter / fadeTime);

            yield return null;
        }
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

    public IEnumerator Overcharge()
    {
        currentFireRate = _ps.overchargeFireRate;
        _ps.hasOvercharge = true;
        
        yield return new WaitForSeconds(_ps.overchargeDuration);

        currentFireRate = fireRate;
        _ps.hasOvercharge = false;
    }
}
