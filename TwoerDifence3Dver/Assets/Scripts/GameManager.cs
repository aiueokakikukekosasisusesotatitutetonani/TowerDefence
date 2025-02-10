using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //シングルトン

<<<<<<< HEAD
    public float delayTime = 3.0f;
    public Text countdownText; //UIテキスト

    public GameObject pAttacker; //味方（attacker）
    public GameObject eAttacker; //敵　 (attacker)

    [SerializeField] private UIManager uIManager;
    void Start()
    {
        StartCoroutine(PauseGame());
        
=======
    public float delayTime = 3.0f;  　//カウントダウン
    public Text countdownText;     　 //UIテキスト
    public Animator gameOverAnim;  　 //GameOver時のアニメーター
    public Animator gameClearAnim;    //GameClear時のアニメーター

    public GameObject pAttacker; //味方（attacker）
    public GameObject pTank;     //味方（tanker）
    public GameObject eAttacker; //敵　 (attacker)
    public GameObject eTank;     //敵   (tanker)

    [SerializeField] private PlayerTower playerTower;
    [SerializeField] private EnemyTower enemyTower;
    void Start()
    {
        gameOverAnim = GameObject.Find("GameOverUI").GetComponent<Animator>();

        StartCoroutine(PauseGame());
        //StartCoroutine(EnemyAttackerSpawnTime());
        //EAttackerSpawn();
        StartCoroutine(EnemyTankSpawnTime());
        //ETankSpawn();

>>>>>>> origin/main
    }

    void Update()
    {
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EAttackerSpawn();
        }
=======
        GameOver();
        GameClear();
>>>>>>> origin/main
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

<<<<<<< HEAD
        while(remainingTime > 0)
=======
        while (remainingTime > 0)
>>>>>>> origin/main
        {
            countdownText.text = Mathf.Ceil(remainingTime).ToString(); //テキスト更新

            yield return new WaitForSecondsRealtime(1.0f); //Ⅰ秒待機

            remainingTime--; //残り時間を減らす
        }
<<<<<<< HEAD
     
=======

>>>>>>> origin/main
        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = ""; //テキストを消す
        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1; // 時間を再開
    }

<<<<<<< HEAD
=======
    public void GameOver()
    {
        if(playerTower.towerHp <= 0)
        {
            gameOverAnim.SetBool("isGameOver", true);
            StartCoroutine(StopGame());         
        }
        
    }

    public void GameClear()
    {
        if(enemyTower.towerHp <= 0)
        {
            gameClearAnim.SetBool("isGameClear", true);
            StartCoroutine(StopGame());
        }
    }

    public IEnumerator StopGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
    }



>>>>>>> origin/main
    public void PAttackerOnClick() //PlayerAttackerを出現させるメソッド
    {
        Vector3 playerAttackerSpawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        //インスタンス化して生成
        GameObject playerAttackerInstance = Instantiate(pAttacker, playerAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
<<<<<<< HEAD
=======

    public void PTTankOnClick()   //PlayerTankを出現させるメソッド
    {
        Vector3 playerTankSpawnPosition = new Vector3(-41.34f, 17.65f, -19.28f);
        //インスタンス化して生成
        GameObject playerTankInstance = Instantiate(pTank, playerTankSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
>>>>>>> origin/main
    public void EAttackerSpawn() //EnemyAttackerを出現させるメソッド
    {
        Vector3 enemyAttackerSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //インスタンス化して生成
        GameObject enemyAttackerInstance = Instantiate(eAttacker, enemyAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
<<<<<<< HEAD
=======

    public void ETankSpawn()
    {
        Vector3 enemyTankSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //インスタンス化して生成
        GameObject enemyTankInstance = Instantiate(eTank, enemyTankSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }

    public IEnumerator EnemyAttackerSpawnTime()
    {
        while (true)
        {
            EAttackerSpawn();
            yield return new WaitForSeconds(15f);
        }
    }

    public IEnumerator EnemyTankSpawnTime()
    {
        while (true)
        {
            ETankSpawn();
            yield return new WaitForSeconds(15f);
        }
    }
>>>>>>> origin/main
}
