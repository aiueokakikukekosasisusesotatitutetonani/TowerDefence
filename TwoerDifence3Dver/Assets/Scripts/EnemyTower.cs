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
    private Coroutine damageCoroutine; // ダメージ処理のコルーチンを保持

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("APlayer"))
        {
            pAttackerScript = other.GetComponent<PAttackerScript>();

            // すでにダメージ処理が動いていなければ開始
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }
        else if (other.CompareTag("TPlayer"))
        {
            pTankScript = other.GetComponent<PTankScript>();

            //すでにダメージ処理が動いていなければ開始
            if (damageCoroutine == null)
            {
                Debug.Log("Tankがifのなかに入りました");
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("APlayer"))
        {
            // ダメージを受ける Coroutine を停止
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        if (other.CompareTag("TPlayer"))
        {
            Debug.Log("Objectが離れました。");

            //ダメージを受けるCoroutineを停止
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
            TowerTakeDamage(pAttackerScript.playerAttackerPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }
        damageCoroutine = null; // コルーチンの参照をリセット
    }

    private IEnumerator TDamageOverTime()
    {
        while(towerHp > 0)
        {
            TowerTakeDamage(pTankScript.playerTackPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }
        Debug.Log("while文を抜けました");
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
