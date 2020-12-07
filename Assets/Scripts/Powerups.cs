using System;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [Header("Powerup Settings")] 
    public Type powerupType;
    
    private PlayerStats _ps;

    [Header("Visual Settings")] 
    public float rotationSpeed = 5f;

    private void Start()
    {
        _ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
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
        Heal
    }
}
