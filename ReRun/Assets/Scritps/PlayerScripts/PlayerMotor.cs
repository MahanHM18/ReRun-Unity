using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpForce;
    public float SlideSpeed;
    public float SoftTime;

    private Vector3 _coruchScale = new Vector3(1, 0.5f, 1);
    private Vector3 _mainScale;

    public bool IsCrouching { get; private set; }

    private PlayerInputManager _input;

    private float _moveSpeed;
    private Rigidbody _rb;

    public Transform GroundCheck;
    public float GroundDistance;
    public LayerMask GroundLayer;

    private CapsuleCollider _capsuleCollider;

    private PlayerAudio _audio;

    private bool _isLand;
    private PlayerLook _look;
    public bool CanDoubleJump { get; set; }
    private bool _isDoubleJump;

    public Vector3 SpawnPoint { private set; get; }

    private bool _back;

    public GameObject Sword;

    public bool IsMoving
    {
        get { return _rb.velocity.magnitude > 0.1; }
    }

    public bool IsRuning { private set; get; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<PlayerAudio>();
        _input = GetComponent<PlayerInputManager>();
        _look = GetComponent<PlayerLook>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        _moveSpeed = WalkSpeed;
        _mainScale = transform.localScale;
        IsCrouching = false;
        _isLand = true;
        CanDoubleJump = false;

        SpawnPoint = transform.position;
        Sword.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!CheckGround())
        {
            _isLand = false;
        }

        if (CheckGround())
        {
            _isDoubleJump = false;
        }

        if (_back)
        {
            transform.position = Vector3.Lerp(transform.position, SpawnPoint, Time.deltaTime * SoftTime);
        }
    }

    public void Move(Vector2 input)
    {
        if (!IsCrouching)
        {
            float speed = Time.deltaTime * 100 * _moveSpeed;

            float x = input.x * speed;
            float z = input.y * speed;

            Vector3 moveDir = _look.Orientation.forward * z + _look.Orientation.right * x;


            Vector3 Lerp = Vector3.Lerp(new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z),
                new Vector3(moveDir.x, _rb.velocity.y, moveDir.z), Time.deltaTime * 2);

            _rb.velocity = Lerp;
        }
    }

    public bool CheckGround()
    {
        bool isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundLayer);


        if (!_isLand && isGrounded && _rb.velocity.y < 0)
        {
            _audio.PlayLandAudio();
            _isLand = true;
        }

        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        if (GroundCheck)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GroundCheck.position, GroundDistance);
        }
    }

    public void Jump()
    {
        if (CheckGround())
        {
            Debug.Log("Jump");
            _audio.PlayJumpAudio();
            _rb.velocity = new Vector3(_rb.velocity.x, JumpForce, _rb.velocity.z);
        }

        if (!CheckGround() && !_isDoubleJump && CanDoubleJump)
        {
            Debug.Log("DobuleJump");
            _isDoubleJump = true;
            _rb.velocity = new Vector3(_rb.velocity.x, JumpForce, _rb.velocity.z);
            _audio.PlayJumpAudio();
        }
    }

    public void StartRunning()
    {
        IsRuning = true;
        _moveSpeed = RunSpeed;
        Debug.Log("StartRunning");
    }

    public void StopRunning()
    {
        IsRuning = false;
        _moveSpeed = WalkSpeed;
        Debug.Log("StopRuning");
    }

    public void StartCrouching()
    {
        IsCrouching = true;
        _capsuleCollider.height = 1;
        _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, _capsuleCollider.center.y - 0.5f,
            _capsuleCollider.center.z);
        transform.GetChild(0).transform.localPosition = new Vector3(0, 0.2f, 0);

        if (_rb.velocity.magnitude > 2f)
        {
            _rb.AddForce(_look.Orientation.forward * SlideSpeed, ForceMode.Force);
            Debug.Log("Slide");
            _audio.PlayStartSlideAudio();
            return;
        }

        Debug.Log("Crouch");
    }

    public void StopCrouching()
    {
        IsCrouching = false;
        _capsuleCollider.height = 2;
        _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, _capsuleCollider.center.y + 0.5f,
            _capsuleCollider.center.z);
        transform.GetChild(0).transform.localPosition = new Vector3(0, 0.6f, 0);
    }

    public void BackToSpawnPoint(string txt)
    {
        _back = true;
        _rb.useGravity = false;
        _rb.velocity = new Vector3(0, 0, 0);
        _input.DisableInput();
        _audio.PlayPickUpClip();
        PPController.Instance.StartEffect();
        UIManager.Instance.SetPowerUp(true, txt);
        StartCoroutine(FinishBack());
    }

    IEnumerator FinishBack()
    {
        yield return new WaitForSeconds(1);
        _back = false;
        _rb.useGravity = true;
        PPController.Instance.EndEffect();
        UIManager.Instance.SetPowerUp(false, "");
        _input.EnableInput();
    }
}