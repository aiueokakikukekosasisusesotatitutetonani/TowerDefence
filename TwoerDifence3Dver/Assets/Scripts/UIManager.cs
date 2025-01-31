using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Probability probability;

    [SerializeField] private float fadeDuration = 1.0f; //�t�F�[�h�C������
    public Image image0; // �t�F�[�h�C����������image
    public Image image1; // �t�F�[�h�C����������image

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        // ������Ԃ𓧖��ɐݒ�
        Color color = image0.color;
        color.a = 0;
        image0.color = color;

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            // �o�ߎ��Ԃɉ����ē����x�𑝉�
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image0.color = color;

            yield return null; //���̃t���[���܂ő҂�
        }

        //���S�ɕs����
        color.a = 1;
        image0.color = color;

        yield return new WaitForSeconds(1f);

        while (elapsedTime < fadeDuration)
        {
            // �o�ߎ��Ԃɉ����ĕs�����x�𑝉�
            elapsedTime -= Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image0.color = color;

            yield return null; //���̃t���[���܂ő҂�
        }

        //���S�ɓ���
        color.a = 0;
        image0.color = color;
    }

    public IEnumerator CimageFadeIn()
    {
        int radValue = probability.rad;

        if (radValue >= 1 && radValue <= 100)
        {
            // ������Ԃ𓧖��ɐݒ�
            Color color = image1.color;
            color.a = 0;
            image1.color = color;

            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                // �o�ߎ��Ԃɉ����ē����x�𑝉�
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                image1.color = color;

                yield return null; //���̃t���[���܂ő҂�
            }

            //���S�ɕs����
            color.a = 1;
            image1.color = color;

            while (elapsedTime < fadeDuration)
            {
                // �o�ߎ��Ԃɉ����ē����x�𑝉�
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                image1.color = color;

                yield return null; //���̃t���[���܂ő҂�
            }
        }
        else
        {
            Debug.Log(radValue);
        }

    }
}
