using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform enemyTarget;

    private List<GameObject> spawnedAttackers = new List<GameObject>();

    public int attackerEnemyHealth = 100; // �L�����N�^�[�̗̑�
    public int enemyAttackPower = 10; // �L�����N�^�[�̍U��

    [SerializeField] private Animator attackerEnemyAnimator; // Animator
    [SerializeField] private Animator tankEnemyAnimator; // Animator
    [SerializeField] private Animator wizardEnemyAnimator; // Animator
    [SerializeField] private GameObject attackerEnemy; // �A�^�b�J�[�̃v���n�u
    [SerializeField] private GameObject tankEnemy; // �^���N�̃v���n�u
    [SerializeField] private GameObject wizardEnemy; // ���@�g���̃v���n�u

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
        attackerEnemyHealth -= damage; // �_���[�W�������̗͂����炷
        Debug.Log("Attacker HP: " + attackerEnemyHealth); // HP�̕ω������O�ŕ\��
        if (attackerEnemyHealth <= 0)
        {
            AttackerEnemyDie(enemyAttackerInstance);
        }
    }

    public virtual void AttackerEnemyDie(GameObject enemyAttackerInstance)
    {
        if (spawnedAttackers.Contains(enemyAttackerInstance))
        {
            spawnedAttackers.Remove(enemyAttackerInstance); // ���X�g����폜
            Destroy(enemyAttackerInstance); // �C���X�^���X���폜
        }
    }

    void EnemyWalkA()
    {
        if (enemyTarget != null && enemyAttackerAgent != null)
        {
            enemyAttackerAgent.isStopped = false; // �ړ����ĊJ
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

        // �C���X�^���X�����X�g�ɒǉ�
        spawnedAttackers.Add(enemyAttackerInstance);

        // NavMesh ��ɃG�[�W�F���g���z�u����Ă��邩�m�F
        if (enemyAttackerAgent.isOnNavMesh)
        {
            enemyAttackerAgent.isStopped = true; // ������Ԃł͈ړ���~
        }
        else
        {
            Debug.LogError("EnemyAttacker �� NavMesh �ɔz�u����Ă��܂���I");
        }
    }

    void EnemySpawnB()
    {
        // �X�|�[������|�W�V����
        Vector3 spawnPosition = new Vector3(6.66f, 17.648f, -19.467f);
        // �C���X�^���X
        GameObject enemyTankInstance = Instantiate(tankEnemy, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        // �A�j���[�^�[�̃R���|�[�l���g
        tankEnemyAnimator = enemyTankInstance.GetComponent<Animator>();
        // NavMesh�̃R���|�[�l���g
        enemyTankAgent = enemyTankInstance.GetComponent<NavMeshAgent>();
        // �ړ���
        enemyTankAgent.isStopped = true; // ������Ԃł͈ړ���~
    }

    void EnemySpawnC()
    {
        // �X�|�[������|�W�V����
        Vector3 spawnPosition = new Vector3(6.66f, 17.648f, -19.467f);
        // �C���X�^���X
        GameObject enemyWizardInstance = Instantiate(wizardEnemy, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        // �A�j���[�^�[�̃R���|�[�l���g
        wizardEnemyAnimator = enemyWizardInstance.GetComponent<Animator>();
        // NavMesh�̃R���|�[�l���g
        enemyWizardAgent = enemyWizardInstance.GetComponent<NavMeshAgent>();
        // �ړ���
        enemyWizardAgent.isStopped = true; // ������Ԃł͈ړ���~
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h�iAttackerPlayer�p�j
    public void OnNotifyCollisionAttackerEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                attackerEnemyAnimator.SetTrigger("AttackerEnemyAttack");

                //�퓬����
                PlayerParameter player = other.GetComponent<PlayerParameter>();
                if (player != null)
                {
                    Debug.Log("Enemy �R���|�[�l���g�𐳏�Ɏ擾���܂����B");
                    AttackerEnemyTakeDamage(player.attackerPower, sender); // �_���[�W����
                }
                else
                {
                    Debug.LogWarning("Enemy �R���|�[�l���g��������܂���B�Փ˂����I�u�W�F�N�g�̐ݒ���m�F���Ă��������B");
                }
                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'AttackerPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h�iTankPlayer�p�j
    public void OnNotifyCollisionTankEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                tankEnemyAnimator.SetTrigger("TankEnemyAttack");

                //�퓬����

                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'TankPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h�iWizardPlayer�p�j
    public void OnNotifyCollisionWizardEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                wizardEnemyAnimator.SetTrigger("WizardEnemyAttack");

                //�퓬����

                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'WizardPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h �����iAttacker�p�j
    public void OnNotifyCollisionAllyAttackerEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'TankPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h �����iTank�p�j
    public void OnNotifyCollisionAllyTankEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'TankPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h �����iWizardPlayer�p�j
    public void OnNotifyCollisionAllyWizardEnemy(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardEnemy"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                Debug.Log($"{sender.name} �� {other.name} �ƏՓ˂��A�ړ����~���ł��B");
            }
            else
            {
                Debug.LogWarning($"{sender.name} �� NavMeshAgent ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"{sender.name} �� 'WizardPlayer' �^�O���ݒ肳��Ă��܂���B");
        }
    }

    //Trigger�̗̈���O�ꂽ�ʒm���󂯎�郁�\�b�h

    public void ResumeMovementEnemy()
    {
        // �Փ˔���𔲂����A�܂�G��|�����̂ŁA�^���[�Ɍ������O�i���ĊJ����B���s�A�j���[�V�����B���̃L�����������ĕ��s�ĊJ
        if (enemyAttackerAgent != null)
        {
            Debug.Log("���s���ĊJ���܂�");
            enemyAttackerAgent.isStopped = false; // Attacker resumes movement
            //attackerAnimator.SetTrigger("AttackerWalk"); // ���s�A�j���[�V�����ĊJ
        }

        if (enemyTankAgent != null)
        {
            enemyTankAgent.isStopped = false; // Tank resumes movement
            //tankAnimator.SetTrigger("TankWalk"); // ���s�A�j���[�V�����ĊJ
        }

        if (enemyWizardAgent != null)
        {
            enemyWizardAgent.isStopped = false; // Wizard resumes movement
            //wizardAnimator.SetTrigger("WizardWalk"); // ���s�A�j���[�V�����ĊJ
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
