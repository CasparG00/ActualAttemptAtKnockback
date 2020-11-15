﻿using UnityEngine;

namespace PostProcessing
{
    public class SpeedLines : MonoBehaviour
    {
        public Transform playerTf;

        [Header("Particle Origin Settings")]
        public float distFromPlayer = 10;

        private Vector3 _currentPlayerPos;
        private Vector3 _lastPlayerPos;

        private void Start()
        {
            _lastPlayerPos = playerTf.position;
        }

        private void Update()
        {
            //Set current Position of player
            _currentPlayerPos = playerTf.position;

            //Find angle between the current player position and the last player position
            var dir = _currentPlayerPos - _lastPlayerPos;

            //Update position and rotation
            transform.position = _currentPlayerPos + dir * distFromPlayer;
            transform.rotation = Quaternion.LookRotation(-dir, Vector3.up);
            // transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);

            //Save position in last frame
            _lastPlayerPos = _currentPlayerPos;
        }
    }
}