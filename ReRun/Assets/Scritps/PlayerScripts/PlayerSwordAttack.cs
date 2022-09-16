using UnityEngine;
using Random = UnityEngine.Random;
using EZCameraShake;

public class PlayerSwordAttack : MonoBehaviour
{
    private Animator _animator;
    private string[] _swingAnimations = new string[2];

    public Transform AttackPoint;
    public float AttackDistance;
    public LayerMask EnemyLayer;

    private bool _canAttack;
    private PlayerAudio _audio;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = transform.parent.transform.parent.GetComponent<PlayerAudio>();

        _swingAnimations[0] = "Swing1";
        _swingAnimations[1] = "Swing2";
    }

    private void Start()
    {
        _canAttack = true;
    }

    public void Attack()
    {
        if (gameObject.activeInHierarchy && _canAttack)
        {
            _animator.Play(_swingAnimations[Random.Range(0, _swingAnimations.Length)]);
            HitEnemy();
            _audio.PlayAttackClip();
            CameraShaker.Instance.ShakeOnce(8f, 4f, 0.4f, 0.5f);
        }
    }

    public void Active()
    {
        _canAttack = false;
    }

    public void Deactive()
    {
        _canAttack = true;
    }

    public void OnDrawGizmos()
    {
        if (AttackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackDistance);
        }
    }

    private void HitEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(AttackPoint.position, AttackDistance, EnemyLayer);

        foreach (Collider item in colliders)
        {
            Debug.Log($"Enemy name is : {item.gameObject.name}");
            float damage = Random.Range(10, 20);
            var health = item.GetComponent<Health>();
            health.TakeDamage(damage);
            UIManager.Instance.SetDamageAmout(damage);
            health.BloodPosition = transform.position;
            StartCoroutine(UIManager.Instance.ActiveHitMarker());
            _audio.PlayHitEnemyClip();
        }
    }
}