using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //�V���O���g��

    public float delayTime = 3.0f;
    public Text countdownText; //UI�e�L�X�g

    public GameObject pAttacker; //�����iattacker�j
    public GameObject eAttacker; //�G�@ (attacker)

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
        Time.timeScale = 0; // ���Ԓ�~

        float remainingTime = delayTime; //�J�E���g�_�E�������l�ݒ�

        while (remainingTime > 0)
        {
            countdownText.text = Mathf.Ceil(remainingTime).ToString(); //�e�L�X�g�X�V

            yield return new WaitForSecondsRealtime(1.0f); //�T�b�ҋ@

            remainingTime--; //�c�莞�Ԃ����炷
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(0.5f);
        countdownText.text = ""; //�e�L�X�g������
        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1; // ���Ԃ��ĊJ
    }

    public void PAttackerOnClick() //PlayerAttacker���o�������郁�\�b�h
    {
        Vector3 playerAttackerSpawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        //�C���X�^���X�����Đ���
        GameObject playerAttackerInstance = Instantiate(pAttacker, playerAttackerSpawnPosition, Quaternion.Euler(0f, 180f, 0f));
    }
    public void EAttackerSpawn() //EnemyAttacker���o�������郁�\�b�h
    {
        Vector3 enemyAttackerSpawnPosition = new Vector3(6.6f, 17.651f, -19.67f);
        //�C���X�^���X�����Đ���
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
