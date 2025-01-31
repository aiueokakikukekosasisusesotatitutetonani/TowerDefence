using UnityEngine;
using System.Collections;

public class PAttackerScript : MonoBehaviour
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
    public int playerAttackerHp = 100;         // �v���C���[�A�^�b�J�[��HP
    public int playerAttackerPower = 10;       // �v���C���[�A�^�b�J�[�̍U����
    private float damageInterval = 1.0f;  // **�_���[�W���󂯂�Ԋu�i�b�j**
    private float lastDamageTime = 0f;    // **�Ō�Ƀ_���[�W���󂯂�����**

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator�̃R���|�[�l���g���擾
        startPosition = transform.position;  // �����ʒu��ݒ�
        isMove = true;                       // isMove��true;
    }

    void Update()
    {
        Debug.Log(isPaused);

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

    void PATakeDamage(int damage)
    {
        //Debug.Log("Player�c��HP:" + playerAttackerHp);
        // **��莞�Ԃ��ƂɃ_���[�W��^����**
        if (Time.time - lastDamageTime >= damageInterval)
        {
            playerAttackerHp -= damage;
            lastDamageTime = Time.time; // **�_���[�W���󂯂����Ԃ��X�V**
        }
        if (playerAttackerHp <= 0)
        {
            Die();
            isMove = false; // isMove��false
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EAttackerScript eAttackerScript = other.GetComponent<EAttackerScript>();

            isPaused = true;  // �ړ����ꎞ��~
            animator.SetBool("isWalk", false);  //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�ړ����Ă���Ƃ��U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�

            PATakeDamage(eAttackerScript.enemyAttackerPower);

            if (playerAttackerHp <= 0)
            {
                eAttackerScript.EAResumeMovement();
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

            if (playerAttackerHp <= 0)
            {
                PAResumeMovement();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PAResumeMovement();
        }
    }

    public void PAResumeMovement()
    {
        Debug.Log("�s���ĊJ");

        isPaused = false; // �ړ����ĊJ
        Debug.Log(isPaused);
        animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
        animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
        animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�
    }
}