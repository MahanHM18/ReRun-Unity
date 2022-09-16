using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject PowerUp;

    public static UIManager Instance;

    public Image HitMarker;

    private Vector2 hitMarkerScale;
    private Vector2 _enemyDeadTextScale;

    private Text _damageAmout;

    public Text EnemyDeadText;
    public string[] DeadMessage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _damageAmout = HitMarker.transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        hitMarkerScale = new Vector2(0, 0);
        _enemyDeadTextScale = new Vector2(0, 0);
    }

    private void Update()
    {
        HitMarker.transform.localScale = Vector3.Lerp(HitMarker.transform.localScale,
            new Vector3(hitMarkerScale.x, hitMarkerScale.y, 0), Time.deltaTime * 9);
        EnemyDeadText.transform.localScale = Vector3.Lerp(EnemyDeadText.transform.localScale,
            new Vector3(_enemyDeadTextScale.x, _enemyDeadTextScale.y, 0), Time.deltaTime * 9);
    }

    public void SetPowerUp(bool active, string text)
    {
        PowerUp.SetActive(active);
        PowerUp.GetComponent<Text>().text = text;
    }

    public IEnumerator ActiveHitMarker()
    {
        HitMarker.gameObject.SetActive(true);

        hitMarkerScale = new Vector2(1, 1);

        yield return new WaitForSeconds(0.4f);

        hitMarkerScale = new Vector2(0, 0);
        
    }

    public void SetDamageAmout(float value)
    {
        _damageAmout.text = value.ToString();
    }

    public IEnumerator ShowEnemyDead()
    {
        EnemyDeadText.gameObject.SetActive(true);

        _enemyDeadTextScale = new Vector2(1, 1);

        EnemyDeadText.text = DeadMessage[Random.Range(0, DeadMessage.Length)];

        yield return new WaitForSeconds(0.7f);

        _enemyDeadTextScale = new Vector2(0, 0);

    }
}