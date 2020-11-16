using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;

    [Header("Shake Settings")]
    public ShakeTransform st;

    public CameraShakeEvent data;
    [Space] 
    public float maxShake = 3f;
    public float speedThreshold = 80;
    
    private float _fallTime;
    
    private void Update() {
        transform.position = player.transform.position;
    }

    private void LateUpdate()
    {
        Shake();
    }

    private void Shake()
    {
        if (rb.velocity.magnitude >= speedThreshold)
        {
            _fallTime += Time.deltaTime * 0.1f;
            _fallTime = Mathf.Clamp(_fallTime, 0, maxShake);
            st.AddShakeEvent(_fallTime, _fallTime, data.duration, data.blendOverLifetime, data.target);
        }
        else
        {
            _fallTime = 0;
        }
    }
}