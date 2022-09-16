using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBox : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var motor = other.GetComponent<PlayerMotor>();
            motor.Sword.SetActive(true);
            motor.BackToSpawnPoint("Sword");
            Destroy(gameObject);
        }
    }
}