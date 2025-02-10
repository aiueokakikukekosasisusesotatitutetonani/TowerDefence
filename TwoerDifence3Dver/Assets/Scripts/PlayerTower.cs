using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public int towerHp = 500;
    public float damageInterval = 1f;
    public GameObject playerTower;
    public GameObject explosion;

    public EAttackerScript eAttackerScript;
    public ETankScript eTankScript;
    private Coroutine damageCoroutine; // �_���[�W�����̃R���[�`����ێ�

    void Start()
    {
        explosion.SetActive(false);
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("AEnemy"))
        {
            eAttackerScript = other.GetComponent<EAttackerScript>();

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }
        else if (other.CompareTag("TEnemy"))
        {
            eTankScript = other.GetComponent<ETankScript>();

            if(damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AEnemy"))
        {
            // �_���[�W���󂯂� Coroutine ���~
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        if (other.CompareTag("TEnemy"))
        {
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ADamageOverTime()
    {
        while (towerHp > 0) // HP��0�ɂȂ�܂Ń��[�v
        {
            TowerTakeDamage(eAttackerScript.enemyAttackerPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    private IEnumerator TDamageOverTime()
    {
        while (towerHp > 0) // HP��0�ɂȂ�܂Ń��[�v
        {
            TowerTakeDamage(eTankScript.enemyTackPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    public void TowerTakeDamage(int damage)
    {
        towerHp -= damage;

        if (towerHp <= 0)
        {
            TowerBroken();
        }
    }

    public void TowerBroken()
    {
        explosion.SetActive(true);
        Destroy(playerTower, 1f);
        Debug.Log("YOU LOSE");
    }
}
