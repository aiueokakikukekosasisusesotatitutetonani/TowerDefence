using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTankScript : MonoBehaviour
{
    public float speed = 5f;                   // �ړ����x
    public float rotationSpeed = 200f;         // ��]���x
    public float[] segmentDistances = { 2f, 200f, 2f }; // �e�Z�O�����g�̈ړ�����
    private int currentSegment = 0;            // ���݂̃Z�O�����g�C���f�b�N�X
    private Vector3 startPosition;             // �e�Z�O�����g�̊J�n�ʒu
    private bool isTurning = false;            // ��]���t���O
    public bool isMove = false;                // Enemy�̈ړ����ĊJ���郁�\�b�h�̃t���O
    private bool isPaused = false;             // �ړ����ꎞ��~����t���O
    public Animator animator;                  // Animator�R���|�[�l���g
    public int playerTankHp = 100;         // �v���C���[�^���N��HP
    public int playerTackPower = 10;       // �v���C���[�^���N�̍U����
    private float damageInterval = 1.0f;       // **�_���[�W���󂯂�Ԋu�i�b�j**

    private Coroutine damageCoroutine; // �_���[�W�����̃R���[�`����ێ�

    public EAttackerScript eAttackerScript;
    public ETankScript eTankScript;

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator�̃R���|�[�l���g���擾
        startPosition = transform.position;  // �����ʒu��ݒ�
    }

    void Update()
    {
        //Debug.Log("Update���̃t���O:" + isPaused);

        if (isPaused) return; // �ꎞ��~���Ȃ珈�����Ȃ�

        if (!isTurning && currentSegment < segmentDistances.Length)
        {
            animator.SetBool("isWalk", true); //�ړ����Ă���Ƃ����s�A�j���[�V���������s
            animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�

            float movedDistance = Vector3.Distance(startPosition, transform.position);
            if (movedDistance < segmentDistances[currentSegment] - 0.01f)  // �덷�␳
            {
                // �w�苗���ɒB����܂ňړ�
                float remainingDistance = segmentDistances[currentSegment] - movedDistance;
                float moveStep = Mathf.Min(speed * Time.deltaTime, remainingDistance); // �K�v�ȏ�ɐi�܂Ȃ��悤�␳
                transform.position += transform.forward * moveStep;
            }
            else
            {
                // ���̃Z�O�����g������ꍇ�̂݉�]
                if (currentSegment < segmentDistances.Length - 1)
                {
                    StartCoroutine(Turn90Degrees());
                }
                else
                {
                    // �Ō�̃Z�O�����g���I�������X�N���v�g�𖳌������ďI��
                    enabled = false;
                }
            }
        }
    }

    private IEnumerator Turn90Degrees()
    {
        isTurning = true;

        float targetAngle = transform.eulerAngles.y - 90f; // �ڕW�p�x
        float currentAngle = transform.eulerAngles.y;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, step);
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);
            yield return null;
        }

        // ���m��90�x��]
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        // �V�����Z�O�����g�̊J�n�ʒu���X�V
        startPosition = transform.position;

        // ���̈ړ��t�F�[�Y
        currentSegment++;

        isTurning = false;
    }

    void PTTakeDamage(int damage)
    {

        playerTankHp -= damage;
        //Debug.Log(playerAttackerHp);

        if (playerTankHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.Translate(-4f, 0f, 0f);
        Destroy(gameObject, 1f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AEnemy"))
        {
            eAttackerScript = other.GetComponent<EAttackerScript>();

            isPaused = true;  // �ړ����ꎞ��~
            //Debug.Log("OnTriggerEnter��: isPaused = " + isPaused);
            animator.SetBool("isWalk", false);  //�ړ��A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ҋ@�A�j���[�V�����𖳌�

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                Debug.Log("DamageOverTime������");
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }

        if (other.CompareTag("TEnemy"))
        {
            eTankScript = other.GetComponent<ETankScript>();

            isPaused = true;  // �ړ����ꎞ��~
            //Debug.Log("OnTriggerEnter��: isPaused = " + isPaused);
            animator.SetBool("isWalk", false);  //�ړ��A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ҋ@�A�j���[�V�����𖳌�

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                Debug.Log("DamageOverTime������");
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }

        if (other.CompareTag("Player"))
        {
            if (transform.position.z > other.transform.position.z)
            {
                return;
            }
            isPaused = true;                       //�ړ���~  
            animator.SetBool("isWalk", false);     //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", false);   //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isIdel", true);      //�ړ����Ă���Ƃ��ҋ@�A�j���[�V���������s

        }

        if (other.CompareTag("EnemyTower"))
        {
            StartCoroutine(Wait());
            isPaused = true;                      //�ړ����~
            animator.SetBool("isWalk", false);    //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true);  //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
            animator.SetBool("isIdel", false);     //�ړ����Ă���Ƃ��ҋ@�A�j���[�V���������s
        }
    }

    // ��莞�Ԃ��ƂɃ_���[�W���󂯂� Coroutine
    private IEnumerator ADamageOverTime()
    {
        while (playerTankHp > 0) // HP��0�ɂȂ�܂Ń��[�v
        {
            Debug.Log("while");
            PTTakeDamage(eAttackerScript.enemyAttackerPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }

        // HP��0�ɂȂ�����G�̈ړ����ĊJ
        Debug.Log("HP0��");
        eAttackerScript.EAResumeMovement();
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    private IEnumerator TDamageOverTime()
    {
        while (playerTankHp > 0) // HP��0�ɂȂ�܂Ń��[�v
        {
            Debug.Log("while");
            PTTakeDamage(eTankScript.enemyTackPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }

        // HP��0�ɂȂ�����G�̈ړ����ĊJ
        Debug.Log("HP0��");
        eTankScript.ETResumeMovement();
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AEnemy"))
        {
            // �_���[�W���󂯂� Coroutine ���~
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            PTResumeMovement(); // �ʏ�̈ړ����ĊJ
        }

        if (other.CompareTag("TEnemy"))
        {
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            PTResumeMovement(); // �ʏ�̈ړ����ĊJ
        }
    }

    public void PTResumeMovement()
    {
        Debug.Log("player�̈ړ��ĊJ");
        isPaused = false; // �ړ����ĊJ
        animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
        animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
        animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
