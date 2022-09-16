using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerLook))]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerMotor _playerMovement;
    private PlayerLook _playerLook;
    private PlayerSwordAttack _playerSwordAttack;

    private void Awake()
    {
        _input = new PlayerInput();
        _playerMovement = GetComponent<PlayerMotor>();
        _playerLook = GetComponent<PlayerLook>();
        _playerSwordAttack = transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerSwordAttack>();

        _input.Player.Jump.performed += ctx => _playerMovement.Jump();

        _input.Player.Run.performed += ctx => _playerMovement.StartRunning();
        _input.Player.Run.canceled += ctx => _playerMovement.StopRunning();

        _input.Player.Crouch.performed += ctx => _playerMovement.StartCrouching();
        _input.Player.Crouch.canceled += ctx => _playerMovement.StopCrouching();

        _input.Player.Attack.performed += ctx => _playerSwordAttack.Attack();

        _input.Player.Shield.performed += ctx => _playerSwordAttack.EnableShield();
        _input.Player.Shield.canceled += ctx => _playerSwordAttack.DisableShield();
    }

    void FixedUpdate()
    {
        _playerMovement.Move(_input.Player.Movement.ReadValue<Vector2>());
    }

    private void Update()
    {
        _playerLook.ApplyLooking(_input.Player.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        EnableInput();
    }

    public void DisableInput()
    {
        _input.Disable();
    }

    public void EnableInput()
    {
        _input.Enable();
    }
}