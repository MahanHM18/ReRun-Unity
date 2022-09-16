using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpBox : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var motor = other.GetComponent<PlayerMotor>();
            motor.CanDoubleJump = true;
            motor.BackToSpawnPoint("Double Jump");

            Destroy(gameObject);
        }
    }
}