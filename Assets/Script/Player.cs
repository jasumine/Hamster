using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed;
    private float maxXPos = 2.8f;
    private float minXPos = -2.8f;

    GameManager manager;

    private void Start()
    {
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

        // player의 좌표가 max보다 커지면 min으로 돌아가도록
        // min보다 작아지면 max로 돌아가도록 설정
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
