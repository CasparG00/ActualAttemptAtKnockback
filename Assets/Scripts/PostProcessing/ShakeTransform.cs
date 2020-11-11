using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeTransform : MonoBehaviour
{
    private class ShakeEvent
    {
        private readonly float _duration;
        private float _timeRemaining;

        private readonly CameraShakeEvent _data;

        public CameraShakeEvent.Target target => _data.target;

        private Vector3 noiseOffset;
        public Vector3 noise;

        public ShakeEvent(CameraShakeEvent data)
        {
            _data = data;

            _duration = data.duration;
            _timeRemaining = _duration;

            const float rand = 32f;

            noiseOffset.x = Random.Range(0f, rand);
            noiseOffset.y = Random.Range(0f, rand);
            noiseOffset.z = Random.Range(0f, rand);
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;

            _timeRemaining -= deltaTime;

            var noiseOffsetDelta = deltaTime * _data.frequency;

            noiseOffset.x += noiseOffsetDelta;
            noiseOffset.y += noiseOffsetDelta;
            noiseOffset.z += noiseOffsetDelta;

            noise.x = Mathf.PerlinNoise(noiseOffset.x, 0f);
            noise.y = Mathf.PerlinNoise(noiseOffset.y, 1f);
            noise.z = Mathf.PerlinNoise(noiseOffset.z, 2f);

            noise -= Vector3.one * 0.5f;

            noise *= _data.amplitude;
            
            var agePercent = 1f - (_timeRemaining / _duration);
            noise *= _data.blendOverLifetime.Evaluate(agePercent);
        }

        public bool IsAlive()
        {
            return _timeRemaining > 0f;
        }
    }

    private readonly List<ShakeEvent> _shakeEvents = new List<ShakeEvent>();

    public void AddShakeEvent(CameraShakeEvent data)
    {
        _shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, CameraShakeEvent.Target target)
    {
        var data = ScriptableObject.CreateInstance<CameraShakeEvent>();
        data.Init(amplitude, frequency, duration, blendOverLifetime, target);
        
        AddShakeEvent(data);
    }

    private void LateUpdate()
    {
        var positionOffset = Vector3.zero;
        var rotationOffset = Vector3.zero;

        for (var i = _shakeEvents.Count - 1; i != -1; i--)
        {
            var se = _shakeEvents[i]; se.Update();

            if (se.target == CameraShakeEvent.Target.Position)
            {
                positionOffset += se.noise;
            }
            else
            {
                rotationOffset += se.noise;
            }

            if (!se.IsAlive())
            {
                _shakeEvents.RemoveAt(i);
            }
        }

        var transform1 = transform;
        transform1.localPosition = positionOffset;
        transform1.localEulerAngles = rotationOffset;
    }
}
