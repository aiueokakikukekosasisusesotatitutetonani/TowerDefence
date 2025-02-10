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
    private Coroutine damageCoroutine; // ダメージ処理のコルーチンを保持

    void Start()
    {
        explosion.SetActive(false);
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("AEnemy"))
        {
            eAttackerScript = other.GetComponent<EAttackerScript>();

            // すでにダメージ処理が動いていなければ開始
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
            // ダメージを受ける Coroutine を停止
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
        while (towerHp > 0) // HPが0になるまでループ
        {
            TowerTakeDamage(eAttackerScript.enemyAttackerPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }
        damageCoroutine = null; // コルーチンの参照をリセット
    }

    private IEnumerator TDamageOverTime()
    {
        while (towerHp > 0) // HPが0になるまでループ
        {
            TowerTakeDamage(eTankScript.enemyTackPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }
        damageCoroutine = null; // コルーチンの参照をリセット
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
