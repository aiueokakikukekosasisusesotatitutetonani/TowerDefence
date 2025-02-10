using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrier : MonoBehaviour
{
    public int barrierHp = 100;   //バリア耐久値



    private void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMissile") || other.CompareTag("PlayerRock"))
        {

            StartCoroutine(BarrierTakeDamage());
        }
    }

    public IEnumerator BarrierTakeDamage()
    {
        barrierHp -= 100;
        if (barrierHp <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
        yield return new WaitForSeconds(1f);
    }
}
