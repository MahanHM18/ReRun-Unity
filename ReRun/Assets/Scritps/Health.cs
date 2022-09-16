using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.UI;

public enum Character
{
    PLAYER,
    ENEMY
};


public class Health : MonoBehaviour
{
    public Character CharacterHealth;

    public float HealthAmout = 100;

    public Image HealthBar;

    private PlayerAudio _audio;

    private EnemyAI _enemyAI;

    public GameObject Blood;

    public Vector3 BloodPosition;

    private void Awake()
    {
        if (CharacterHealth == Character.PLAYER)
            _audio = GetComponent<PlayerAudio>();
        if (CharacterHealth == Character.ENEMY)
            _enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float value)
    {
        HealthAmout -= value;

        if (HealthBar)
        {
            HealthBar.fillAmount = HealthAmout / 100;
        }

        if (CharacterHealth == Character.PLAYER)
        {
            StartCoroutine(PPController.Instance.ActiveDamagetEffect());
            Debug.Log("Player dameged");
            CameraShaker.Instance.ShakeOnce(12, 4, 0.6f, 0.5f);
            _audio.PlayHitPlayerClip();
            GetComponent<Rigidbody>().AddForce(-GetComponent<PlayerLook>().Orientation.forward * 10,ForceMode.Impulse);
        }

        if (CharacterHealth == Character.ENEMY)
        {
            Instantiate(Blood, BloodPosition, Quaternion.identity);

            if (HealthAmout <= 0)
            {
                _enemyAI.Death();
                HealthBar.transform.parent.gameObject.SetActive(false);
                StartCoroutine(UIManager.Instance.ShowEnemyDead());
            }
                
        }
            
    }
}