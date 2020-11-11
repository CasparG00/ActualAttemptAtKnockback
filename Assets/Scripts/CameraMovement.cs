using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform player;
    public Rigidbody rb;

    [Header("Shake Settings")]
    public ShakeTransform st;
    public CameraShakeEvent data;
    [Space]
    public float maxShake = 3f;

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
        if (rb.velocity.y < -20)
        {
            _fallTime += Time.deltaTime;
            _fallTime = Mathf.Clamp(_fallTime, 0, maxShake);
            st.AddShakeEvent(_fallTime, _fallTime, data.duration, data.blendOverLifetime, data.target);
        }
        else
        {
            _fallTime = 0;
        }
    }
    
    public IEnumerator SmoothTranslate(Vector3 origin, Vector3 target, float duration, AnimationCurve curve)
    {
        var journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;
            
            var percent = Mathf.Clamp01(journey / duration);
            var curvePercent = curve.Evaluate(percent);
            transform.position += Vector3.LerpUnclamped(origin, target, curvePercent);
    
            yield return null;
        }
    }
}