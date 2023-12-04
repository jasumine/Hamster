using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTXT;
    public GameObject SettingPanel;
    public GameObject StartImage;

    public GameObject boxParents;
    public float boxRotationSpeed;
    public float maxRotate;
    public float minRotate;

    public bool isShake;

    void Start()
    {
        isShake = false;
        ShowScore(0);
    }

    public void ShowScore(int score)
    {
        scoreTXT.text = score.ToString();
    }

    public void TouchToStart()
    {
        StartImage.SetActive(false);
    }

    


    // ��ư�� �̿��ؼ� ����
    public void ShakeBox()
    {
        if (isShake == false)
        {
            Debug.Log("�ڽ� ȸ�� ����");
            StartCoroutine("ShakeBoxZ");
        }
    }
    IEnumerator ShakeBoxZ()
    {
        isShake = true;

        float z = 0;

        int count = 0;
        while (count <= 3)
        {
            while (z <= maxRotate)
            {
                z += Time.deltaTime * boxRotationSpeed;
                boxParents.transform.rotation = Quaternion.Euler(0, 0, z);
            }

            yield return new WaitForSeconds(0.5f);

            while (z >= minRotate)
            {
                z -= Time.deltaTime * boxRotationSpeed;
                boxParents.transform.rotation = Quaternion.Euler(0, 0, z);
            }
            count++;
            yield return new WaitForSeconds(0.5f);
        }

        boxParents.transform.rotation = Quaternion.Euler(0, 0, 0);
        isShake = false;
    }

    public void ActiveSettingPopUp()
    {
        SettingPanel.SetActive(true);
    }

    public void UnActiveSettingPopUp()
    {
        SettingPanel.SetActive(false);
    }



}
