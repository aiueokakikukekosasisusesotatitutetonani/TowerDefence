using UnityEngine;
using System.Collections;

public class EAttackerScript : MonoBehaviour
{
    public float speed = 5f;                   // �ړ����x
    public float rotationSpeed = 200f;         // ��]���x
    public float[] segmentDistances = { 2f, 200f, 2f }; // �e�Z�O�����g�̈ړ�����
    private int currentSegment = 0;            // ���݂̃Z�O�����g�C���f�b�N�X
    private Vector3 startPosition;             // �e�Z�O�����g�̊J�n�ʒu
    private bool isTurning = false;            // ��]���t���O
    private bool isPaused = false;             // �ړ����ꎞ��~����t���O
<<<<<<< HEAD
    public Animator animator;                  // animator
    public int enemyAttackerHp = 100;          // �G�l�~�[�A�^�b�J�[��HP
    public int enemyAttackerPower = 10;        // �G�l�~�[�A�^�b�J�[�̍U����
    private float damageInterval = 1.0f;  // **�_���[�W���󂯂�Ԋu�i�b�j**
    private float lastDamageTime = 0f;    // **�Ō�Ƀ_���[�W���󂯂�����**

    public PAttackerScript pAttackerScript;
=======
    public bool isMove = false;                // �ړ����ĊJ���郁�\�b�h���󂯎��t���O
    public Animator animator;                  // animator
    public int enemyAttackerHp = 100;          // �G�l�~�[�A�^�b�J�[��HP
    public int enemyAttackerPower = 10;        // �G�l�~�[�A�^�b�J�[�̍U����
    private float damageInterval = 1.0f;       // **�_���[�W���󂯂�Ԋu�i�b�j**

    private Coroutine damageCoroutine; // �_���[�W�����̃R���[�`����ێ�

    public PAttackerScript pAttackerScript;          // player�̃X�N���v�g
    public PTankScript pTankScript;                  // tank�̃X�N���v�g
>>>>>>> origin/main

    void Start()
    {
        animator = GetComponent<Animator>(); // animator�̃R���|�[�l���g���擾
        startPosition = transform.position;  // �����ʒu��ݒ�
    }

    void Update()
    {
<<<<<<< HEAD
        Debug.Log(isPaused);
=======
        //Debug.Log(isPaused);
>>>>>>> origin/main

        if (isPaused) return; // �ꎞ��~���Ȃ珈�����Ȃ�

        if (!isTurning && currentSegment < segmentDistances.Length)
        {
<<<<<<< HEAD
            animator.SetBool("isWalk", true); //�ړ����Ă���Ƃ����s�A�j���[�V���������s
            animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
=======
            animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
            animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
            animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�
>>>>>>> origin/main

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

        float targetAngle = transform.eulerAngles.y + 90f; // �ڕW�p�x
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

    void EATakeDamage(int damage)
    {
        enemyAttackerHp -= damage;
<<<<<<< HEAD
        //Debug.Log("Enemy�c��HP:" + enemyAttackerHp);
        if (enemyAttackerHp <= 0)
        {
            Die();
            pAttackerScript.PAResumeMovement(); // �ړ��ĊJ
=======

        //Debug.Log(enemyAttackerHp);

        if (enemyAttackerHp <= 0)
        {
            Die();
            isMove = false;
>>>>>>> origin/main
        }
    }

    public void Die()
    {
<<<<<<< HEAD
        Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPaused = true;  // �ړ����ꎞ��~
            animator.SetBool("isWalk", false); //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�ړ����Ă���Ƃ��U���A�j���[�V���������s

            // **��莞�Ԃ��ƂɃ_���[�W��^����**
            if (Time.time - lastDamageTime >= damageInterval)
            {
                EATakeDamage(pAttackerScript.playerAttackerPower);
                lastDamageTime = Time.time; // **�_���[�W���󂯂����Ԃ��X�V**
            }
        }
=======
        transform.Translate(10f, 0f, 0f);
        Destroy(gameObject, 1f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("APlayer"))
        {
            pAttackerScript = other.GetComponent<PAttackerScript>();

            isPaused = true;  // �ړ����ꎞ��~
            animator.SetBool("isWalk", false);  //�ړ��A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ҋ@�A�j���[�V�����𖳌�

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }
        else if (other.CompareTag("TPlayer"))
        {
            pTankScript = other.GetComponent<PTankScript>();

            isPaused = true;  // �ړ����ꎞ��~
            animator.SetBool("isWalk", false);  //�ړ��A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ҋ@�A�j���[�V�����𖳌�

            // ���łɃ_���[�W�����������Ă��Ȃ���ΊJ�n
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }

        if (other.CompareTag("Enemy"))  //�����ƏՓ˂��Ă����
        {
            if (transform.position.z > other.transform.position.z)
            {
                return;
            }
            isPaused = true;                      //�ړ����~
            animator.SetBool("isWalk", false);    //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", false);  //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
            animator.SetBool("isIdel", true);     //�ړ����Ă���Ƃ��ҋ@�A�j���[�V���������s

        }

        if (other.CompareTag("PlayerTower"))
        {
            isPaused = true;                      //�ړ����~
            animator.SetBool("isWalk", false);    //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true);  //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
            animator.SetBool("isIdel", false);     //�ړ����Ă���Ƃ��ҋ@�A�j���[�V���������s
        }
    }

    // ��莞�Ԃ��ƂɃ_���[�W���󂯂� Coroutine
    private IEnumerator ADamageOverTime()  //Attacker�ɍU�����󂯂�
    {
        while (enemyAttackerHp > 0) // HP��0�ɂȂ�܂Ń��[�v
        {
            EATakeDamage(pAttackerScript.playerAttackerPower);
            yield return new WaitForSeconds(damageInterval); // �w�莞�ԑ҂�
        }

        // HP��0�ɂȂ�����G�̈ړ����ĊJ
        pAttackerScript.PAResumeMovement();
        damageCoroutine = null; // �R���[�`���̎Q�Ƃ����Z�b�g
    }

    private IEnumerator TDamageOverTime() //Tank�ɍU�����󂯂�
    {
        while(enemyAttackerHp > 0)
        {
            EATakeDamage(pTankScript.playerTackPower);
            yield return new WaitForSeconds(damageInterval); //�w��b���҂�
        }

        //HP��0�ɂȂ�����G�̈ړ����ĊJ
        pTankScript.PTResumeMovement();
        damageCoroutine = null;
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit");

        if (other.CompareTag("APlayer"))
        {
            // �_���[�W���󂯂� Coroutine ���~
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            EAResumeMovement(); // �ʏ�̈ړ����ĊJ
        }

        if (other.CompareTag("TPlayer"))
        {
            // �_���[�W���󂯂� Coroutine���~
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            EAResumeMovement(); //�ʏ�ړ����J�n
        }
>>>>>>> origin/main
    }

    public void EAResumeMovement()
    {
<<<<<<< HEAD
        isPaused = false; // �ړ����ĊJ
        animator.SetBool("isWalk", true); //�ړ����Ă���Ƃ����s�A�j���[�V���������s
        animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
=======
        //Debug.Log("Enemy�̈ړ��ĊJ");
        isPaused = false; // �ړ����ĊJ
        //Debug.Log(isPaused);
        animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
        animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
        animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�
>>>>>>> origin/main
    }
}