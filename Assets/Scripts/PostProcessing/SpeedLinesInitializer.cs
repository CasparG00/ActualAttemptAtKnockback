using UnityEngine;

namespace PostProcessing
{
    public class SpeedLinesInitializer : MonoBehaviour
    {
        public GameObject particleEmitter;
        public float speedThreshold = 30;
        
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            particleEmitter.SetActive(_rb.velocity.magnitude >= speedThreshold);
        }
    }
}
