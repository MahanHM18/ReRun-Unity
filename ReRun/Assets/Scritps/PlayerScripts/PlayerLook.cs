using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private Transform _look;

    private float _xRotation = 0;
    private float _yRotation;

    public float Sensitivity;
    public Transform Orientation;

    private void Awake()
    {
        _look = transform.GetChild(0).transform;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
    }


    public void ApplyLooking(Vector2 input)
    {
        float MouseX = input.x * Time.deltaTime * Sensitivity;
        float MouseY = input.y * Time.deltaTime * Sensitivity;


        _xRotation -= MouseY;
        _yRotation += MouseX;

        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        _look.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        Orientation.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }
}