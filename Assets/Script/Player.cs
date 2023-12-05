using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float maxXPos;
    public float minXPos;

    GameManager manager;

    private bool isShake;

    private void Start()
    {
        isShake = false;
        manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        PlayerInput();
    }


    private void PlayerInput()
    {
        if (manager.ismove)
        {
            Move();
        }
        else return;
    }

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
