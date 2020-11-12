using UnityEngine;

public class FallGuard : MonoBehaviour
{
    public Vector3 respawnPoint = new Vector3(0, 2, 0);

    public float maxFallDistance = -200;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y <= -200)
        {
            transform.position = respawnPoint;
            _rb.velocity = Vector3.zero;
        }
    }
}
