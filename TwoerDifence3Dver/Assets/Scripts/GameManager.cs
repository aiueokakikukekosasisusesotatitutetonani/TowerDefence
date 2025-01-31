using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //シングルトン

    public float delayTime = 3.0f;
    public Text countdownText; //UIテキスト

    public GameObject pAttacker; //味方（attacker）
    public GameObject eAttacker; //敵　 (attacker)

    [SerializeField] private UIManager uIManager;
    void Start()
    {
        StartCoroutine(PauseGame());
        StartCoroutine(EnemyAttackerSpawnTime());
    }

    void Update()
    {

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator PauseGame()
    {
        Time.timeScale = 0; // 時間停止

        float remainingTime = delayTime; //カウントダウン初期値設定

        while (remainingTime > 0)
        {
            countdownText.text = Mathf.Ceil(remainingTime).ToString(); //テキスト更新

            yield return new WaitForSecondsRealtime(1.0f); //Ⅰ秒待機

            remainingTime--; //残り時間を減らす
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = ""; //テキストを消す
        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1; // 時間を再開
    }

    public void PAttackerOnClick() //PlayerAttackerを出現させるメソッド
    {
        Vector3 playerAttackerSpawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        //インスタンス化して生成
        GameObject playerAttackerInstance = Instantiate(pAttacker, playerAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
    public void EAttackerSpawn() //EnemyAttackerを出現させるメソッド
    {
        Vector3 enemyAttackerSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //インスタンス化して生成
        GameObject enemyAttackerInstance = Instantiate(eAttacker, enemyAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }

    public IEnumerator EnemyAttackerSpawnTime()
    {
        while (true)
        {
            EAttackerSpawn();
            yield return new WaitForSeconds(15f);

        }

    }
}
