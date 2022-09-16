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

    private PlayerSwordAttack _swordAttack;

    private void Awake()
    {
        if (CharacterHealth == Character.PLAYER)
        {
            _audio = GetComponent<PlayerAudio>();
            _swordAttack = transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerSwordAttack>();
        }

        if (CharacterHealth == Character.ENEMY)
        {
            _enemyAI = GetComponent<EnemyAI>();
        }
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
            if (_swordAttack.HasShield)
            {
                CameraShaker.Instance.ShakeOnce(12, 4, 0.6f, 0.5f);
                GetComponent<Rigidbody>()
                    .AddForce(-GetComponent<PlayerLook>().Orientation.forward * 5, ForceMode.Impulse);
                Debug.Log("Shield dameged");
            }
            else
            {
                StartCoroutine(PPController.Instance.ActiveDamagetEffect());
                Debug.Log("Player dameged");
                _audio.PlayHitPlayerClip();
                GetComponent<Rigidbody>()
                    .AddForce(-GetComponent<PlayerLook>().Orientation.forward * 10, ForceMode.Impulse);
            }
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