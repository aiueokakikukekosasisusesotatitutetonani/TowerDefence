using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //�V���O���g��

<<<<<<< HEAD
    public float delayTime = 3.0f;
    public Text countdownText; //UI�e�L�X�g

    public GameObject pAttacker; //�����iattacker�j
    public GameObject eAttacker; //�G�@ (attacker)

    [SerializeField] private UIManager uIManager;
    void Start()
    {
        StartCoroutine(PauseGame());
        
=======
    public float delayTime = 3.0f;  �@//�J�E���g�_�E��
    public Text countdownText;     �@ //UI�e�L�X�g
    public Animator gameOverAnim;  �@ //GameOver���̃A�j���[�^�[
    public Animator gameClearAnim;    //GameClear���̃A�j���[�^�[

    public GameObject pAttacker; //�����iattacker�j
    public GameObject pTank;     //�����itanker�j
    public GameObject eAttacker; //�G�@ (attacker)
    public GameObject eTank;     //�G   (tanker)

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
        Time.timeScale = 0; // ���Ԓ�~

        float remainingTime = delayTime; //�J�E���g�_�E�������l�ݒ�

<<<<<<< HEAD
        while(remainingTime > 0)
=======
        while (remainingTime > 0)
>>>>>>> origin/main
        {
            countdownText.text = Mathf.Ceil(remainingTime).ToString(); //�e�L�X�g�X�V

            yield return new WaitForSecondsRealtime(1.0f); //�T�b�ҋ@

            remainingTime--; //�c�莞�Ԃ����炷
        }
<<<<<<< HEAD
     
=======

>>>>>>> origin/main
        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = ""; //�e�L�X�g������
        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1; // ���Ԃ��ĊJ
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
    public void PAttackerOnClick() //PlayerAttacker���o�������郁�\�b�h
    {
        Vector3 playerAttackerSpawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        //�C���X�^���X�����Đ���
        GameObject playerAttackerInstance = Instantiate(pAttacker, playerAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
<<<<<<< HEAD
=======

    public void PTTankOnClick()   //PlayerTank���o�������郁�\�b�h
    {
        Vector3 playerTankSpawnPosition = new Vector3(-41.34f, 17.65f, -19.28f);
        //�C���X�^���X�����Đ���
        GameObject playerTankInstance = Instantiate(pTank, playerTankSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
>>>>>>> origin/main
    public void EAttackerSpawn() //EnemyAttacker���o�������郁�\�b�h
    {
        Vector3 enemyAttackerSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //�C���X�^���X�����Đ���
        GameObject enemyAttackerInstance = Instantiate(eAttacker, enemyAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
<<<<<<< HEAD
=======

    public void ETankSpawn()
    {
        Vector3 enemyTankSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //�C���X�^���X�����Đ���
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
