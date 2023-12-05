using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTXT;

    public GameObject RankPanel;

    public GameObject InfoPanel;
    public Image[] infoImages;
    public TextMeshProUGUI[] infoText;
    public int currentPage;

    public GameObject SettingPanel;
    public GameObject StartImage;
    public GameObject EndImage;

    public GameObject boxParents;
    public float boxRotationSpeed;
    public float maxRotate;
    public float minRotate;

    public bool isShake;

    void Start()
    {
        isShake = false;
        currentPage = 0;

        StartImage.SetActive(true);
        EndImage.SetActive(false);
    }

    public void ShowScore(int score)
    {
        scoreTXT.text = score.ToString();
    }

    public void TouchToStart()
    {
        StartImage.SetActive(false);
    }

    


    // 버튼을 이용해서 실행
    public void ShakeBox()
    {
        if (isShake == false)
        {
            Debug.Log("박스 회전 실행");
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

    public void ActiveRankPopUp()
    {
        RankPanel.SetActive(true);
    }
    public void UnActiveRankPopUp()
    {
        RankPanel.SetActive(false);
    }


    public void ActiveInfoPopUP()
    {
        InfoPanel.SetActive(true);
    }

    public void UnActiveInfoPopUP()
    {
        InfoPanel.SetActive(false);
    }

    public void MoveBeforeInfoPage()
    {
        if (currentPage > 0 && currentPage <= infoImages.Length-1)
        {
            currentPage--;
            for(int i =0; i<infoImages.Length; i++)
            {
                if(i==currentPage)
                {
                    infoImages[i].gameObject.SetActive(true);
                    infoText[i].color = new Color32(121, 226, 181, 255);
                }
                else
                {
                    infoImages[i].gameObject.SetActive(false);
                    infoText[i].color = new Color32(205, 231, 220, 255);
                }
            }
        }
        else
        {
            return;
        }
    }
    public void MoveAfterInfoPage()
    {
        if (currentPage >= 0 && currentPage < infoImages.Length-1 )
        {
            currentPage++;
            for (int i = 0; i < infoImages.Length; i++)
            {
                if (i == currentPage)
                {
                    infoImages[i].gameObject.SetActive(true);
                    infoText[i].color = new Color32(121, 226, 181, 255);
                }
                else
                {
                    infoImages[i].gameObject.SetActive(false);
                    infoText[i].color = new Color32(205,231,220,255);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void ActiveSettingPopUp()
    {
        SettingPanel.SetActive(true);
    }

    public void UnActiveSettingPopUp()
    {
        SettingPanel.SetActive(false);
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
