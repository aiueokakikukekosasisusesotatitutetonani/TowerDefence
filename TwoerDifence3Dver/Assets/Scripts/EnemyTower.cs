using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : MonoBehaviour
{
    public int towerHp = 500;
    public float damageInterval = 1f;
    public GameObject enemyTower;
    public GameObject explosion;

    public PAttackerScript pAttackerScript;
    public PTankScript pTankScript;
    private Coroutine damageCoroutine; // �_���[�W�����̃R���[�`����ێ�

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("APlayer"))
        {
            pAttackerScript = other.GetComponent<PAttackerScript>();

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }
        else if (other.CompareTag("TPlayer"))
        {
            pTankScript = other.GetComponent<PTankScript>();

            //���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                Debug.Log("Tank��if�̂Ȃ��ɓ���܂���");
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("APlayer"))
        {
            // �_���[�W���󂯂� Coroutine ���~
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        if (other.CompareTag("TPlayer"))
        {
            Debug.Log("Object������܂����B");

            //�_���[�W���󂯂�Coroutine���~
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
            TowerTakeDamage(pAttackerScript.playerAttackerPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    private IEnumerator TDamageOverTime()
    {
        while(towerHp > 0)
        {
            TowerTakeDamage(pTankScript.playerTackPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }
        Debug.Log("while���𔲂��܂���");
        damageCoroutine = null;
    }

    public void TowerTakeDamage(int damage)
    {
        towerHp -= damage;
        Debug.Log(towerHp);
        if (towerHp <= 0)
        {
            TowerBroken();
        }
    }

    public void TowerBroken()
    {
        explosion.SetActive(true);
        Destroy(enemyTower, 1f);
        Debug.Log("YOU WIN");
    }

    public void WaitTime()
    {
        TowerTakeDamage(pTankScript.playerTackPower);
    }
}
