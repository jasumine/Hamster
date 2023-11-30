using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTXT;
    public GameObject boxParents;

    void Start()
    {
        ShowScore(0);
    }

    public void ShowScore(int score)
    {
        scoreTXT.text = score.ToString();
    }

    // ��ư�� �̿��ؼ� ����
    //private void ShakeBox()
    //{
    //    // �ڽ� ����
    //    Quaternion quaternion;

    //    Vector3 targetPosition = boxParents.transform.position;
    //    Vector3 rightVector = Vector3.right;
    //    Quaternion rotation = Quaternion.LookRotation(targetPosition, rightVector);

    //}
}
