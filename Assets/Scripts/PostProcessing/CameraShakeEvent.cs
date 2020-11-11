using UnityEngine;

[CreateAssetMenu(fileName = "Shake Transform Event", menuName = "Custom/Shake Transform Event", order = 1)]
public class CameraShakeEvent : ScriptableObject
{
    public enum Target
    {
        Position,
        Rotation
    }

    public Target target = Target.Position;

    public float amplitude = 1f;
    public float frequency = 1f;
    public float duration = 1f;

    public AnimationCurve blendOverLifetime = new AnimationCurve(
        new Keyframe(0f, 0f, Mathf.Deg2Rad * 0f, Mathf.Deg2Rad * 720f),
        new Keyframe(0.2f, 1f),
        new Keyframe(1f, 0f));

    public void Init(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, Target target)
    {
        this.target = target;

        this.amplitude = amplitude;
        this.frequency = frequency;
        
        this.duration = duration;

        this.blendOverLifetime = blendOverLifetime;
    }
}