using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float maxXPos;
    public float minXPos;

    private bool isShake;

    private void Start()
    {
        isShake = false;
    }

    private void Update()
    {
        PlayerInput();
    }


    private void PlayerInput()
    {
        if (GameManager.GetInstance().ismove)
        {
            Move();
        }
        else return;
    }

    // Ű���忡�� ������ �� < >
    private void Move()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        Vector3 currentPos = transform.position;

        // player�� ��ǥ�� max���� Ŀ���� min���� ���ư�����
        // min���� �۾����� max�� ���ư����� ����
        if(horizontalValue >0)
        {
            currentPos.x += (moveSpeed* 0.01f);
            if (currentPos.x > maxXPos)
                currentPos.x = minXPos;
        }
        else if(horizontalValue < 0)
        {
            currentPos.x -= (moveSpeed * 0.01f);
            if(currentPos.x <minXPos)
                currentPos.x = maxXPos;
        }

        transform.position = currentPos;
    }

}
