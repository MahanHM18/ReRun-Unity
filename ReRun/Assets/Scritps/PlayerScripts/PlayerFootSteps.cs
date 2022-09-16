using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerFootSteps : MonoBehaviour
{
    private PlayerAudio _audio;
    private PlayerMotor _motor;

    public float WalkTime;
    public float RunTime;

    private float _timer;

    private void Awake()
    {
        _audio = GetComponent<PlayerAudio>();
        _motor = GetComponent<PlayerMotor>();
    }
    
    void Update()
    {
        if (_motor.CheckGround() && _motor.IsMoving && !_motor.IsCrouching)
        {
            _timer += Time.deltaTime;

            float MaxTime = _motor.IsRuning ? RunTime : WalkTime;
            
            if (_timer > MaxTime)
            {
                _timer = 0;
                _audio.PlayFootStepsAudio();
            }
        }
    }
}