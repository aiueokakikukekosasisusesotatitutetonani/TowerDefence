using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform playerTarget;

    private List<GameObject> spawnedAttackers = new List<GameObject>();

    public int attackerHealth = 100; // attacker�̗̑�
    public int tankHealth = 100;     // tank�̗̑�
    public int wizardHealth = 100;   // wizard�̗̑�

    [SerializeField] private Animator attackerAnimator; // Animator
    [SerializeField] private Animator tankAnimator; // Animator
    [SerializeField] private Animator wizardAnimator; // Animator
    [SerializeField] private GameObject attacker; // �A�^�b�J�[�̃v���n�u
    [SerializeField] private GameObject tank; // �^���N�̃v���n�u
    [SerializeField] private GameObject wizard; // ���@�g���̃v���n�u
    [SerializeField] private bool canSpown = true; // �X�|�[���Ǘ��t���O
    [SerializeField] private float cooldownTime = 5f; // �N�[���_�E���̕b��

    private NavMeshAgent attackerAgent;
    private NavMeshAgent tankAgent;
    private NavMeshAgent wizardAgent;

    void Start()
    {
    }

    public void AttackerTakeDamage(int damage, GameObject attackerInstance)
    {
        attackerHealth -= damage; // �_���[�W�������̗͂����炷
        //Debug.Log("Attacker HP: " + attackerHealth); // HP�̕ω������O�ŕ\��
        if (attackerHealth <= 0)
        {
            AttackerDie(attackerInstance);
        }
    }


    public virtual void AttackerDie(GameObject attackerInstance)
    {

        if (spawnedAttackers.Contains(attackerInstance))
        {
            spawnedAttackers.Remove(attackerInstance); // ���X�g����폜
            Destroy(attackerInstance); // �C���X�^���X���폜
        }
    }

    void WalkA() //����Walk���\�b�h�͎g�p���Ă��Ȃ�
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
        //�C���X�^���X�����Đ���
        //GameObject attackerInstance = Instantiate(attacker, spawnPosition, Quaternion.Euler(0f, 180f, 0f));

        // �C���X�^���X�����X�g�ɒǉ�

        // �A�j���[�^�[���擾���ă��X�g�ɒǉ�

    }

    void SpawnB()
    {
        Vector3 spawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        GameObject tankInstance = Instantiate(tank, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        tankAnimator = tankInstance.GetComponent<Animator>();
        tankAgent = tankInstance.GetComponent<NavMeshAgent>();
        tankAgent.isStopped = true; // ������Ԃł͈ړ���~
    }

    void SpawnC()
    {
        Vector3 spawnPosition = new Vector3(-41.4f, 17.651f, -19.67f);
        GameObject wizardInstance = Instantiate(wizard, spawnPosition, Quaternion.Euler(0f, 180f, 0f));
        wizardAnimator = wizardInstance.GetComponent<Animator>();
        wizardAgent = wizardInstance.GetComponent<NavMeshAgent>();
        wizardAgent.isStopped = true; // ������Ԃł͈ړ���~
    }

    IEnumerator SpownCooldown()
    {
        canSpown = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpown = true;
    }

    // �Փ˒ʒm���󂯎�郁�\�b�h�iAttackerPlayer�p�j
    public void OnNotifyCollisionAttacker(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                attackerAnimator.SetTrigger("AttackerAttack");

                //�퓬����
                EnemyParameter enemyparameter = other.GetComponent<EnemyParameter>();
                if (enemyparameter != null)
                {
                    Debug.Log("Enemy �R���|�[�l���g�𐳏�Ɏ擾���܂����B");
                    AttackerTakeDamage(enemyparameter.tankEnemyPower, sender); // �_���[�W����
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
    public void OnNotifyCollisionTank(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                tankAnimator.SetTrigger("TankAttack");
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
    public void OnNotifyCollisionWizard(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardPlayer"))
        {
            NavMeshAgent instanceAgent = sender.GetComponent<NavMeshAgent>();
            if (instanceAgent != null)
            {
                instanceAgent.isStopped = true;
                wizardAnimator.SetTrigger("WizardAttack");
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
    public void OnNotifyCollisionAllyAttacker(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("AttackerPlayer"))
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
    public void OnNotifyCollisionAllyTank(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("TankPlayer"))
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
    public void OnNotifyCollisionAllyWizard(GameObject sender, GameObject other)
    {
        if (sender.CompareTag("WizardPlayer"))
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

    public void ResumeMovement()
    {

        // �Փ˔���𔲂����A�܂�G��|�����̂ŁA�^���[�Ɍ������O�i���ĊJ����B���s�A�j���[�V�����B���̃L�����������ĕ��s�ĊJ
        if (attackerAgent != null)
        {
            Debug.Log("���s���ĊJ���܂�");
            attackerAgent.isStopped = false; // Attacker resumes movement
            //attackerAnimator.SetTrigger("AttackerWalk"); // ���s�A�j���[�V�����ĊJ
        }

        if (tankAgent != null)
        {
            tankAgent.isStopped = false; // Tank resumes movement
            //tankAnimator.SetTrigger("TankWalk"); // ���s�A�j���[�V�����ĊJ
        }

        if (wizardAgent != null)
        {
            wizardAgent.isStopped = false; // Wizard resumes movement
            //wizardAnimator.SetTrigger("WizardWalk"); // ���s�A�j���[�V�����ĊJ
        }
    }
}




//����APlayerCharacter���_���[�W���󂯂�B�G��|������O�i���ĊJ
//�v���C���[����������閈�Ƀ^���[�̔����J��
//���,�����L�����N�^�[�����łɓG�ƌ�킵�Ă����Ԃ��ƈړ����̃A�j���[�V�������U���̃A�j���[�V�����ɂȂ�
//OnTriggerExit���G��Destroy�����O�Ɏ��s�ł��Ȃ����߁A����Trigger�𔲂������ƂɂȂ��Ă��Ȃ��B
//���A�j���[�V�����̃o�O�APrefab��Animator��Inspecter�ɐ���������ԐV�����N���[���̃A�j���[�^�[�������Ă��遦
