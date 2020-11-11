using UnityEngine;

public class Pellet : MonoBehaviour
{
    private Vector3 _origin;
    public float maxDistance = 50;
    
    private void Start()
    {
        _origin = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _origin) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}
