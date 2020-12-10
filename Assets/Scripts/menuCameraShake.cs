using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MenuCameraShake : MonoBehaviour
{
    public CameraShakeEvent data;

    [Header("Flinch Settings")]
    [Range(0, 1)]
    public float targetVignetteValue = 0.31f;
    [Range(0, 1)]
    public float targetChromaticAberrationValue = 1f;

    private ShakeTransform _st;
    private Volume _v;

    private Vignette _vg;
    private ChromaticAberration _ca;

    private void Start()
    {
        _st = transform.GetChild(0).GetComponent<ShakeTransform>();
        _v = transform.GetChild(0).GetComponent<Volume>();

        if (_v.profile.TryGet<Vignette>(out var vignette)) _vg = vignette;
        if (_v.profile.TryGet<ChromaticAberration>(out var chromaticAberration)) _ca = chromaticAberration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Projectile"))
        {
            _st.AddShakeEvent(data);
        }

        var currentVignetteValue = _vg.intensity.value;
        StartCoroutine(Flinch(currentVignetteValue, targetVignetteValue, data.duration, data.blendOverLifetime,
            returnedValue => _vg.intensity.value = returnedValue));

        var currentChromaticAberrationValue = _ca.intensity.value;
        StartCoroutine(Flinch(currentChromaticAberrationValue, targetChromaticAberrationValue, data.duration,
            data.blendOverLifetime, returnedValue => _ca.intensity.value = returnedValue));
    }

    private IEnumerator Flinch(float origin, float target, float duration, AnimationCurve curve,
        System.Action<float> callback)
    {
        var journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;

            var percent = Mathf.Clamp01(journey / duration);
            var curvePercent = curve.Evaluate(percent);
            var result = Mathf.SmoothStep(origin, target, curvePercent);

            yield return null;

            callback(result);
        }
    }
}
