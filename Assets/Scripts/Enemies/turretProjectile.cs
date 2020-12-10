using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float lifetime = 10f;
    
    public CameraShakeEvent data;
    
    private PlayerStats _ps;
    private ShakeTransform _st;
    
    private void Start()
    {
        _ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        _st = GameObject.Find("Camera Shake").GetComponent<ShakeTransform>();
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _ps.Damage(1);
            _st.AddShakeEvent(data);
        }

        Destroy(gameObject);
    }
}
