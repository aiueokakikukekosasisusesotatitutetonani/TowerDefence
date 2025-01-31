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
    public bool isMove = false;                // �ړ����ĊJ���郁�\�b�h���󂯎��t���O
    public Animator animator;                  // animator
    public int enemyAttackerHp = 100;          // �G�l�~�[�A�^�b�J�[��HP
    public int enemyAttackerPower = 10;        // �G�l�~�[�A�^�b�J�[�̍U����
    private float damageInterval = 1.0f;  // **�_���[�W���󂯂�Ԋu�i�b�j**
    private float lastDamageTime = 0f;    // **�Ō�Ƀ_���[�W���󂯂�����**

    void Start()
    {
        animator = GetComponent<Animator>(); // animator�̃R���|�[�l���g���擾
        startPosition = transform.position;  // �����ʒu��ݒ�
        isMove = true;                       // isMove��true�ɂ��Ă���
    }

    void Update()
    {
        //Debug.Log(isPaused);

        if (isPaused) return; // �ꎞ��~���Ȃ珈�����Ȃ�

        if (!isTurning && currentSegment < segmentDistances.Length)
        {
            animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
            animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
            animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�

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
        //Debug.Log("Enemy�c��HP:" + enemyAttackerHp);

        // **��莞�Ԃ��ƂɃ_���[�W��^����**
        if (Time.time - lastDamageTime >= damageInterval)
        {
            enemyAttackerHp -= damage;
            lastDamageTime = Time.time; // **�_���[�W���󂯂����Ԃ��X�V**
        }
        if (enemyAttackerHp <= 0)
        {
            Die();
            isMove = false;  //isMove��false�ɂ���
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PAttackerScript pAttackerScript = other.GetComponent<PAttackerScript>();

            isPaused = true;  // �ړ����ꎞ��~
            animator.SetBool("isWalk", false);  //�ړ����Ă���Ƃ����s�A�j���[�V�����𖳌�
            animator.SetBool("isAttack", true); //�ړ����Ă���Ƃ��U���A�j���[�V���������s
            animator.SetBool("isIdel", false);  //�ړ����Ă���Ƃ��ҋ@�A�j���[�V���������s

            EATakeDamage(pAttackerScript.playerAttackerPower);

            if (enemyAttackerHp <= 0)
            {
                Debug.Log("���g");
                pAttackerScript.PAResumeMovement();
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

            if (enemyAttackerHp <= 0)
            {
                EAResumeMovement();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EAResumeMovement();
        }
    }

    public void EAResumeMovement()
    {
        isPaused = false; // �ړ����ĊJ
        Debug.Log(isPaused);
        animator.SetBool("isWalk", true);    //�ړ����Ă���Ƃ����s�A�j���[�V���������s
        animator.SetBool("isAttack", false); //�ړ����Ă���Ƃ��U���A�j���[�V�����𖳌�
        animator.SetBool("isIdel", false);   //�ړ����Ă���Ƃ��ҋ@�A�j���[�V�����𖳌�
    }

    public IEnumerator CurrentTime()
    {
        yield return new WaitForSeconds(2f);
    }
}