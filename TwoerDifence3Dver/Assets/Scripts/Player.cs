using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform playerTarget;

    private List<GameObject> spawnedAttackers = new List<GameObject>();

    public int attackerHealth = 100; // attackerの体力
    public int tankHealth = 100;     // tankの体力
    public int wizardHealth = 100;   // wizardの体力

    [SerializeField] private Animator attackerAnimator; // Animator
    [SerializeField] private Animator tankAnimator; // Animator
    [SerializeField] private Animator wizardAnimator; // Animator
    [SerializeField] private GameObject attacker; // アタッカーのプレハブ
    [SerializeField] private GameObject tank; // タンクのプレハブ
    [SerializeField] private GameObject wizard; // 魔法使いのプレハブ
    [SerializeField] private bool canSpown = true; // スポーン管理フラグ
    [SerializeField] private float cooldownTime = 5f; // クールダウンの秒数

    private NavMeshAgent attackerAgent;
    private NavMeshAgent tankAgent;
    private NavMeshAgent wizardAgent;

    void Start()
    {
    }

    public void AttackerTakeDamage(int damage, GameObject attackerInstance)
    {
        attackerHealth -= damage; // ダメージ分だけ体力を減らす
        //Debug.Log("Attacker HP: " + attackerHealth); // HPの変化をログで表示
        if (attackerHealth <= 0)
        {
            AttackerDie(attackerInstance);
        }
    }


    public virtual void AttackerDie(GameObject attackerInstance)
    {

        if (spawnedAttackers.Contains(attackerInstance))
        {
            spawnedAttackers.Remove(attackerInstance); // リストから削除
            Destroy(attackerInstance); // インスタンスを削除
        }
    }

    void WalkA() //現在Walkメソッドは使用していない
    {
        
    }

    void WalkB()
    {
        if (playerTarget != null && tankAgent != null)
        {
            tankAgent.isStopped = false;
            tankAnimator.SetTrigger("TankWalk");
            tankAgent.SetDestination(playerTarget.position);
        }
    }

    void WalkC()
    {
        if (playerTarget != null && wizardAgent != null)
        {
            wizardAgent.isStopped = false;
            wizardAnimator.SetTrigger("WizardWalk");
            wizardAgent.SetDestination(playerTarget.position);
        }
    }

    public void OnClickA()
    {
        if (canSpown)
        {
            SpawnA();
            WalkA();
            StartCoroutine(SpownCooldown());
        }
    }

    public void OnClickB()
    {
        if (canSpown)
        {
            SpawnB();
            WalkB();
            StartCoroutine(SpownCooldown());
        }
    }

    public void OnClickC()
    {
        if (canSpown)
        {
            SpawnC();
            WalkC();
            StartCoroutine(SpownCooldown());
        }
    }

    void SpawnA()
    {
        //Vector3 spawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        //インスタンス化して生成
        //GameObject attackerInstance = Instantiate(attacker, spawnPosition, Quaternion.Euler(0f, 180f, 0f));

        // インスタンスをリストに追加

        // アニメーターを取得してリストに追加

    }

    void SpawnB()
    {
        Vector3 spawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        GameObject tankInstance = Instantiate(tank, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        tankAnimator = tankInstance.GetComponent<Animator>();
        tankAgent = tankInstance.GetComponent<NavMeshAgent>();
        tankAgent.isStopped = true; // 初期状態では移動停止
    }

    void SpawnC()
    {
        Vector3 spawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        GameObject wizardInstance = Instantiate(wizard, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        wizardAnimator = wizardInstance.GetComponent<Animator>();
        wizardAgent = wizardInstance.GetComponent<NavMeshAgent>();
        wizardAgent.isStopped = true; // 初期状態では移動停止
    }

    IEnumerator SpownCooldown()
    {
        canSpown = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpown = true;
    }

    // 衝突通知を受け取るメソッド（AttackerPlayer用）
    public void OnNotifyCollisionAttacker(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                attackerAnimator.SetTrigger("AttackerAttack");

                //戦闘処理
                EnemyParameter enemyparameter = other.GetComponent<EnemyParameter>();
                if (enemyparameter != null)
                {
                    Debug.Log("Enemy コンポーネントを正常に取得しました。");
                    AttackerTakeDamage(enemyparameter.tankEnemyPower, sender); // ダメージ処理
                }
                else
                {
                    Debug.LogWarning("Enemy コンポーネントが見つかりません。衝突したオブジェクトの設定を確認してください。");
                }


                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'AttackerPlayer' タグが設定されていません。");
        }
    }

    // 衝突通知を受け取るメソッド（TankPlayer用）
    public void OnNotifyCollisionTank(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                tankAnimator.SetTrigger("TankAttack");
                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'TankPlayer' タグが設定されていません。");
        }
    }

    // 衝突通知を受け取るメソッド（WizardPlayer用）
    public void OnNotifyCollisionWizard(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                wizardAnimator.SetTrigger("WizardAttack");
                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'WizardPlayer' タグが設定されていません。");
        }
    }

    // 衝突通知を受け取るメソッド 味方（Attacker用）
    public void OnNotifyCollisionAllyAttacker(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'TankPlayer' タグが設定されていません。");
        }
    }

    // 衝突通知を受け取るメソッド 味方（Tank用）
    public void OnNotifyCollisionAllyTank(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'TankPlayer' タグが設定されていません。");
        }
    }

    // 衝突通知を受け取るメソッド 味方（WizardPlayer用）
    public void OnNotifyCollisionAllyWizard(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} が {other.name} と衝突し、移動を停止中です。");
            }
            else
            {
                Debug.LogWarning($"{sender.name} に NavMeshAgent が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} は 'WizardPlayer' タグが設定されていません。");
        }
    }

    //Triggerの領域を外れた通知を受け取るメソッド

    public void ResumeMovement()
    {

        // 衝突判定を抜けた、つまり敵を倒したので、タワーに向かい前進を再開する。歩行アニメーション。後ろのキャラも続いて歩行再開
        if (attackerAgent != null)
        {
            Debug.Log("歩行を再開します");
            attackerAgent.isStopped = false; // Attacker resumes movement
            //attackerAnimator.SetTrigger("AttackerWalk"); // 歩行アニメーション再開
        }

        if (tankAgent != null)
        {
            tankAgent.isStopped = false; // Tank resumes movement
            //tankAnimator.SetTrigger("TankWalk"); // 歩行アニメーション再開
        }

        if (wizardAgent != null)
        {
            wizardAgent.isStopped = false; // Wizard resumes movement
            //wizardAnimator.SetTrigger("WizardWalk"); // 歩行アニメーション再開
        }
    }
}




//次回、PlayerCharacterがダメージを受ける。敵を倒したら前進を再開
//プレイヤーが召喚される毎にタワーの扉が開く
//問題,同じキャラクターがすでに敵と交戦している状態だと移動中のアニメーションが攻撃のアニメーションになる
//OnTriggerExitが敵がDestroyされる前に実行できないため、実質Triggerを抜けたことになっていない。
//※アニメーションのバグ、PrefabのAnimatorのInspecterに生成した一番新しいクローンのアニメーターが入っている※
