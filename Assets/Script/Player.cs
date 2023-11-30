using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed;
    private float maxXPos = 2.5f;
    private float minXPos = -2.5f;
    public GameObject boxParents;
    public float boxRotationSpeed;
    public float maxRotate;
    public float minRotate;

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
            ShakeBox();
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

    private void ShakeBox()
    {
        if(isShake == false && Input.GetKeyDown(KeyCode.R)) 
        {
            Debug.Log("박스 회전 실행");
            StartCoroutine("ChangeZ");
        }
    }

    IEnumerator ChangeZ()
    {
        isShake = true;

        float z = 0;

        // ===============1==================
        int count = 0;
        while(count <=3)
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
}
