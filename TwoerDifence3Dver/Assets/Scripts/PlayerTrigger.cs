using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Player targetScript; // Player�X�N���v�g�ւ̎Q��
    public GameObject attackerPrefab; 
    public List<Collider> currentCollisions = new List<Collider>(); // �Փ˒���Collider�����X�g�ŊǗ�

    void Start()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<Player>();
            if (targetScript == null)
            {
                Debug.LogError("�V�[������Player�X�N���v�g��������܂���I");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AttackerEnemy") || other.CompareTag("TankEnemy") || other.CompareTag("WizardEnemy")) // �Փ˂������肪Enemy�^�O�Ȃ�
        {
            Debug.Log("OnTriggerStay called for: " + other.name);
            if (!currentCollisions.Contains(other))
            {
                currentCollisions.Add(other); // ���߂ďՓ˂���Enemy�����X�g�ɒǉ�
                Debug.Log(other.name + " �ƏՓ˂��܂����B���X�g�ɒǉ�����܂����B");
            }

            if (targetScript != null)// �����̃^�O�ɉ����āA�K�؂Ȓʒm���\�b�h���Ăяo��
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // ������AttackerPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // ������TankPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // ������WizardPlayer�̏ꍇ
                {
                    targetScript.OnNotifyCollisionWizard(this.gameObject, other.gameObject);
                }
            }
        }

        if (other.CompareTag("AttackerPlayer"))
        {
            if (targetScript != null)
            {
                if (this.gameObject.CompareTag("AttackerPlayer")) // ������AttackerPlayer�̏ꍇ
                {
                    if(transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // ������TankPlayer�̏ꍇ
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
                if (this.gameObject.CompareTag("AttackerPlayer")) // ������AttackerPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // ������TankPlayer�̏ꍇ
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
                if (this.gameObject.CompareTag("AttackerPlayer")) // ������AttackerPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyAttacker(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("TankPlayer")) // ������TankPlayer�̏ꍇ
                {
                    if (transform.position.z < other.transform.position.z)
                    {
                        return;
                    }
                    targetScript.OnNotifyCollisionAllyTank(this.gameObject, other.gameObject);
                }
                else if (this.gameObject.CompareTag("WizardPlayer")) // ������TankPlayer�̏ꍇ
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