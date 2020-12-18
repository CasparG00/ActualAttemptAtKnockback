using System;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [Header("Powerup Settings")] 
    public Type powerupType;
    
    private PlayerStats _ps;
    private Shooting _s;

    [Header("Visual Settings")] 
    public float rotationSpeed = 5f;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _ps = player.GetComponent<PlayerStats>();
        _s = player.GetComponent<Shooting>();
    }

    private void Update()
    {
        Animate();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GivePowerup();
        }
    }

    private void GivePowerup()
    {
        switch (powerupType)
        {
            case Type.FallProtection:
                if (!_ps.hasFallProtection)
                {
                    _ps.hasFallProtection = true;
                    print("Given Fall Protection");
                    Destroy(gameObject);
                }
                break;
            case Type.Heal:
                if (_ps.health < _ps.maxHealth)
                {
                    _ps.Heal(1);
                    print("Given Health");
                    Destroy(gameObject);
                }
                break;
            case Type.SecondChance:
                if (!_ps.hasSecondChance)
                {
                    _ps.hasSecondChance = true;
                    print("Given Second Chance");
                    Destroy(gameObject);
                }
                break;
            case Type.Overcharge:
                if (!_ps.hasOvercharge)
                {
                    _s.StartCoroutine(_s.Overcharge());
                    print("Given Overcharge");
                    Destroy(gameObject);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Animate()
    {
        var rotation = rotationSpeed * Time.deltaTime;
        
        transform.Rotate(rotation, rotation, rotation);
        transform.position += new Vector3(0, Mathf.Sin(Time.time), 0) * Time.deltaTime;
    }

    public enum Type
    {
        FallProtection,
        Heal,
        SecondChance,
        Overcharge
    }
}
