﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")] 
    public float speed = 4500;
    public float maxSpeed = 20;
    public float counterMovement = 0.175f;
    public float airSpeedMultiplier = 0.5f;

    [Space]
    public Transform orientation;
    public CapsuleCollider col;

    [Header("Camera Settings")]
    public Transform cam;
    public float sensitivity;

    public bool lockCursor;

    [Header("Camera Effects Settings")]
    public AnimationCurve impactShake;
    public float camOffsetY = 1;

    private Rigidbody _rb;

    private float _cameraX, _cameraY;
    private const float Threshold = 0.01f;
    private float _x, _z;
    private float _multiplier;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        GetComponent<MeshRenderer>().enabled = false;

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void MyInput()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _z = Input.GetAxisRaw("Vertical");
    }

    private void Movement()
    {
        //Apply counter movement if player is grounded
        if (CheckGround())
        {
            var mag = FindVelRelativeToLook();
            CounterMovement(_x, mag.x, orientation.transform.right);
            CounterMovement(_z, mag.y, orientation.transform.forward);
        }

        _multiplier = CheckGround() ? 1f : airSpeedMultiplier;

        //Move player
        _rb.AddForce(orientation.transform.forward * (_z * speed * _multiplier));
        _rb.AddForce(orientation.transform.right * (_x * speed * _multiplier));
    }

    //Check if player is on ground
    private bool CheckGround()
    {
        return Physics.Raycast(_rb.position, Vector3.down, col.height);
    }

    private float _actualSpeed;

    private void CounterMovement(float inputAxis, float magDir, Vector3 dir)
    {
        //normalize movement if player walks diagonally
        _actualSpeed = Mathf.Abs(_x) + Mathf.Abs(_z) == 2 ? Mathf.Sqrt(maxSpeed * maxSpeed / 2) : maxSpeed;

        if (inputAxis > 0 && magDir > _actualSpeed) inputAxis = 0;
        if (inputAxis < 0 && magDir < -_actualSpeed) inputAxis = 0;

        if (Math.Abs(magDir) > Threshold && Math.Abs(inputAxis) < 0.05f ||
                magDir < -Threshold && inputAxis > 0 ||
                magDir > Threshold && inputAxis < 0)
        {
            _rb.AddForce(dir * (speed * -magDir * counterMovement));
        }
    }

    private Vector2 FindVelRelativeToLook()
    {
        var lookAngle = orientation.transform.eulerAngles.y;
        var velocity = _rb.velocity;
        var moveAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

        var u = Mathf.DeltaAngle(lookAngle, moveAngle);
        var v = 90 - u;

        var magnitude = _rb.velocity.magnitude;
        var yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        var xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void Look()
    {
        var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        var rot = cam.transform.localRotation.eulerAngles;
        _cameraY = rot.y + mouseX;

        _cameraX -= mouseY;
        _cameraX = Mathf.Clamp(_cameraX, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(_cameraX, _cameraY, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, _cameraY, 0);
    }

    private void OnCollisionEnter()
    {
        if (_rb.velocity.y != 0) return;
        var targetPos = new Vector3(0, -camOffsetY, 0);
        StartCoroutine(cam.GetComponent<CameraMovement>().SmoothTranslate(Vector3.zero, targetPos, .25f, impactShake));
    }
}