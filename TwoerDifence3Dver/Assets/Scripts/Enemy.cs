using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform enemyTarget;

    private List<GameObject> spawnedAttackers = new List<GameObject>();

    public int attackerEnemyHealth = 100; // キャラクターの体力
    public int enemyAttackPower = 10; // キャラクターの攻撃

    [SerializeField] private Animator attackerEnemyAnimator; // Animator
    [SerializeField] private Animator tankEnemyAnimator; // Animator
    [SerializeField] private Animator wizardEnemyAnimator; // Animator
    [SerializeField] private GameObject attackerEnemy; // アタッカーのプレハブ
    [SerializeField] private GameObject tankEnemy; // タンクのプレハブ
    [SerializeField] private GameObject wizardEnemy; // 魔法使いのプレハブ

    private NavMeshAgent enemyAttackerAgent;
    private NavMeshAgent enemyTankAgent;
    private NavMeshAgent enemyWizardAgent;

    void Start()
    {
        //StartCoroutine(FirstSpawnTime());
        EnemySpawnA();
        EnemyWalkA();
        
    }

    void Update()
    {
         
    }
    public void AttackerEnemyTakeDamage(int damage, GameObject enemyAttackerInstance)
    {
        attackerEnemyHealth -= damage; // ダメージ分だけ体力を減らす
        Debug.Log("Attacker HP: " + attackerEnemyHealth); // HPの変化をログで表示
        if (attackerEnemyHealth <= 0)
        {
            AttackerEnemyDie(enemyAttackerInstance);
        }
    }

    public virtual void AttackerEnemyDie(GameObject enemyAttackerInstance)
    {
        if (spawnedAttackers.Contains(enemyAttackerInstance))
        {
            spawnedAttackers.Remove(enemyAttackerInstance); // リストから削除
            Destroy(enemyAttackerInstance); // インスタンスを削除
        }
    }

    void EnemyWalkA()
    {
        if (enemyTarget != null && enemyAttackerAgent != null)
        {
            enemyAttackerAgent.isStopped = false; // 移動を再開
            attackerEnemyAnimator.SetTrigger("AttackerEnemyWalk");
            enemyAttackerAgent.SetDestination(enemyTarget.position);
        }
    }

    void EnemyWalkB()
    {
        if (enemyTarget != null && enemyTankAgent != null)
        {
            enemyTankAgent.isStopped = false;
            tankEnemyAnimator.SetTrigger("TankEnemyWalk");
            enemyTankAgent.SetDestination(enemyTarget.position);
        }
    }

    void EnemyWalkC()
    {
        if (enemyTarget != null && enemyWizardAgent != null)
        {
            enemyWizardAgent.isStopped = false;
            wizardEnemyAnimator.SetTrigger("WizardEnemyWalk");
            enemyWizardAgent.SetDestination(enemyTarget.position);
        }
    }

    void EnemySpawnA()
    {
        Vector3 spawnPosition = new Vector3(6.66f, 17.648f, -19.467f);
        GameObject enemyAttackerInstance = Instantiate(attackerEnemy, spawnPosition, Quaternion.Euler(0f, 180f, 0f));

        attackerEnemyAnimator = enemyAttackerInstance.GetComponent<Animator>();
        enemyAttackerAgent = enemyAttackerInstance.GetComponent<NavMeshAgent>();

        // インスタンスをリストに追加
        spawnedAttackers.Add(enemyAttackerInstance);

        // NavMesh 上にエージェントが配置されているか確認
        if (enemyAttackerAgent.isOnNavMesh)
        {
            enemyAttackerAgent.isStopped = true; // 初期状態では移動停止
        }
        else
        {
            Debug.LogError("EnemyAttacker が NavMesh に配置されていません！");
        }
    }

    void EnemySpawnB()
    {
        // スポーンするポジション
        Vector3 spawnPosition = new Vector3(6.66f, 17.648f, -19.467f);
        // インスタンス
        GameObject enemyTankInstance = Instantiate(tankEnemy, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        // アニメーターのコンポーネント
        tankEnemyAnimator = enemyTankInstance.GetComponent<Animator>();
        // NavMeshのコンポーネント
        enemyTankAgent = enemyTankInstance.GetComponent<NavMeshAgent>();
        // 移動可否
        enemyTankAgent.isStopped = true; // 初期状態では移動停止
    }

    void EnemySpawnC()
    {
        // スポーンするポジション
        Vector3 spawnPosition = new Vector3(6.66f, 17.648f, -19.467f);
        // インスタンス
        GameObject enemyWizardInstance = Instantiate(wizardEnemy, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        // アニメーターのコンポーネント
        wizardEnemyAnimator = enemyWizardInstance.GetComponent<Animator>();
        // NavMeshのコンポーネント
        enemyWizardAgent = enemyWizardInstance.GetComponent<NavMeshAgent>();
        // 移動可否
        enemyWizardAgent.isStopped = true; // 初期状態では移動停止
    }

    // 衝突通知を受け取るメソッド（AttackerPlayer用）
    public void OnNotifyCollisionAttackerEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                attackerEnemyAnimator.SetTrigger("AttackerEnemyAttack");

                //戦闘処理
                PlayerParameter player = other.GetComponent<PlayerParameter>();
                if (player != null)
                {
                    Debug.Log("Enemy コンポーネントを正常に取得しました。");
                    AttackerEnemyTakeDamage(player.attackerPower, sender); // ダメージ処理
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
    public void OnNotifyCollisionTankEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                tankEnemyAnimator.SetTrigger("TankEnemyAttack");

                //戦闘処理

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
    public void OnNotifyCollisionWizardEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                wizardEnemyAnimator.SetTrigger("WizardEnemyAttack");

                //戦闘処理

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
    public void OnNotifyCollisionAllyAttackerEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerEnemy"))
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
    public void OnNotifyCollisionAllyTankEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankEnemy"))
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
    public void OnNotifyCollisionAllyWizardEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardEnemy"))
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

    public void ResumeMovementEnemy()
    {
        // 衝突判定を抜けた、つまり敵を倒したので、タワーに向かい前進を再開する。歩行アニメーション。後ろのキャラも続いて歩行再開
        if (enemyAttackerAgent != null)
        {
            Debug.Log("歩行を再開します");
            enemyAttackerAgent.isStopped = false; // Attacker resumes movement
            //attackerAnimator.SetTrigger("AttackerWalk"); // 歩行アニメーション再開
        }

        if (enemyTankAgent != null)
        {
            enemyTankAgent.isStopped = false; // Tank resumes movement
            //tankAnimator.SetTrigger("TankWalk"); // 歩行アニメーション再開
        }

        if (enemyWizardAgent != null)
        {
            enemyWizardAgent.isStopped = false; // Wizard resumes movement
            //wizardAnimator.SetTrigger("WizardWalk"); // 歩行アニメーション再開
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Time());
    }

    public IEnumerator Time()
    {
        yield return new WaitForSeconds(3f);
    }

    public IEnumerator FirstSpawnTime()
    {
        yield return new WaitForSecondsRealtime(0.5f);
    }
}
