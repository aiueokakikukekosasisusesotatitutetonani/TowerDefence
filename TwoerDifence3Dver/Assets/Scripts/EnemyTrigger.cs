using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public Enemy targetScript; // Playerスクリプトへの参照
    public List<Collider> currentCollisions = new List<Collider>(); // 衝突中のColliderをリストで管理

    void Start()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<Enemy>();
            if (targetScript == null)
            {
                Debug.LogError("シーン内にPlayerスクリプトが見つかりません！");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AttackerPlayer") || other.CompareTag("TankPlayer") || other.CompareTag("WizardPlayer")) // 衝突した相手がPlayerタグなら
        {
            Debug.Log("OnTriggerStay called for: " + other.name + "Enemyyyyyyyyyyyy");
            if (!currentCollisions.Contains(other))
            {
                currentCollisions.Add(other); // 初めて衝突したEnemyをリストに追加
                Debug.Log(other.name + " と衝突しました。リストに追加されました。Enemyyyyyyyyyyyyyyyyyyyy");
            }

            if (targetScript != null)// 自分のタグに応じて、適切な通知メソッドを呼び出す
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // 自分がAttackerPlayerの場合
                {
                    targetScript.OnNotifyCollisionAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // 自分がTankPlayerの場合
                {
                    targetScript.OnNotifyCollisionTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // 自分がWizardPlayerの場合
                {
                    targetScript.OnNotifyCollisionWizardEnemy(this.gameObject, other.gameObject);
                }
            }
        }

        //味方にぶつかった時
        if (other.CompareTag("AttackerEnemy"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // 自分がAttackerPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizardEnemy(this.gameObject, other.gameObject);
                }
            }
        }
        else if (other.CompareTag("TankEnemy"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // 自分がAttackerPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizardEnemy(this.gameObject, other.gameObject);
                }
            }
        }
        else if (other.CompareTag("WizardEnemy"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // 自分がAttackerPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizardEnemy(this.gameObject, other.gameObject);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called for: " + other.name);
        // リストから削除し、Destroyされたら移動再開
        if (other.CompareTag("AttackerPlayer") || other.CompareTag("TankPlayer") || other.CompareTag("WizardPlayer"))
        {
            if (currentCollisions.Contains(other))
            {
                currentCollisions.Remove(other); // リストから削除
                Debug.Log(other.name + " がトリガーを抜けました。リストから削除されました。");

                // オブジェクトがDestroyされたとき、移動再開
                if (other.gameObject == null) return;

                // Playerスクリプトに移動再開の通知
                targetScript.ResumeMovementEnemy();
            }
        }
    }
}