using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public Enemy targetScript; // Player�X�N���v�g�ւ̎Q��
    public List<Collider> currentCollisions = new List<Collider>(); // �Փ˒���Collider�����X�g�ŊǗ�

    void Start()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<Enemy>();
            if (targetScript == null)
            {
                Debug.LogError("�V�[������Player�X�N���v�g��������܂���I");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AttackerPlayer") || other.CompareTag("TankPlayer") || other.CompareTag("WizardPlayer")) // �Փ˂������肪Player�^�O�Ȃ�
        {
            Debug.Log("OnTriggerStay called for: " + other.name + "Enemyyyyyyyyyyyy");
            if (!currentCollisions.Contains(other))
            {
                currentCollisions.Add(other); // ���߂ďՓ˂���Enemy�����X�g�ɒǉ�
                Debug.Log(other.name + " �ƏՓ˂��܂����B���X�g�ɒǉ�����܂����BEnemyyyyyyyyyyyyyyyyyyyy");
            }

            if (targetScript != null)// �����̃^�O�ɉ����āA�K�؂Ȓʒm���\�b�h���Ăяo��
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // ������AttackerPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // ������TankPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // ������WizardPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionWizardEnemy(this.gameObject, other.gameObject);
                }
            }
        }

        //�����ɂԂ�������
        if (other.CompareTag("AttackerEnemy"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerEnemy")) // ������AttackerPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // ������TankPlayer�̏ꍇ
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
                if (this.gameObject.CompareTag("AttackerEnemy")) // ������AttackerPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // ������TankPlayer�̏ꍇ
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
                if (this.gameObject.CompareTag("AttackerEnemy")) // ������AttackerPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttackerEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankEnemy")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTankEnemy(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardEnemy")) // ������TankPlayer�̏ꍇ
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
        // ���X�g����폜���ADestroy���ꂽ��ړ��ĊJ
        if (other.CompareTag("AttackerPlayer") || other.CompareTag("TankPlayer") || other.CompareTag("WizardPlayer"))
        {
            if (currentCollisions.Contains(other))
            {
                currentCollisions.Remove(other); // ���X�g����폜
                Debug.Log(other.name + " ���g���K�[�𔲂��܂����B���X�g����폜����܂����B");

                // �I�u�W�F�N�g��Destroy���ꂽ�Ƃ��A�ړ��ĊJ
                if (other.gameObject == null) return;

                // Player�X�N���v�g�Ɉړ��ĊJ�̒ʒm
                targetScript.ResumeMovementEnemy();
            }
        }
    }
}