using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Player targetScript; // Playerスクリプトへの参照
    public GameObject attackerPrefab; 
    public List<Collider> currentCollisions = new List<Collider>(); // 衝突中のColliderをリストで管理

    void Start()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<Player>();
            if (targetScript == null)
            {
                Debug.LogError("シーン内にPlayerスクリプトが見つかりません！");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AttackerEnemy") || other.CompareTag("TankEnemy") || other.CompareTag("WizardEnemy")) // 衝突した相手がEnemyタグなら
        {
            Debug.Log("OnTriggerStay called for: " + other.name);
            if (!currentCollisions.Contains(other))
            {
                currentCollisions.Add(other); // 初めて衝突したEnemyをリストに追加
                Debug.Log(other.name + " と衝突しました。リストに追加されました。");
            }

            if (targetScript != null)// 自分のタグに応じて、適切な通知メソッドを呼び出す
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // 自分がAttackerPlayerの場合
                {
                    targetScript.OnNotifyCollisionAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // 自分がTankPlayerの場合
                {
                    targetScript.OnNotifyCollisionTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // 自分がWizardPlayerの場合
                {
                    targetScript.OnNotifyCollisionWizard(this.gameObject, other.gameObject);
                }
            }
        }

        if (other.CompareTag("AttackerPlayer"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // 自分がAttackerPlayerの場合
                {
                    if(transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizard(this.gameObject, other.gameObject);
                }
            }
        }
        else if (other.CompareTag("TankPlayer"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // 自分がAttackerPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizard(this.gameObject, other.gameObject);
                }
            }
        }
        else if (other.CompareTag("WizardPlayer"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // 自分がAttackerPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // 自分がTankPlayerの場合
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyWizard(this.gameObject, other.gameObject);
                }
            }
        }
    }
}