using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class JellyController : MonoBehaviour
{
    // ���� �ܰ��� ����
    public GameObject nextJelly;
    public Sprite jellyImage;

    public int level;
    private bool checkCollision = false;

    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag =="Jelly")
        {
            JellyController other = collision.gameObject.GetComponent<JellyController>();
            if (other.level == level)
            {

                if (!checkCollision)
                {
                    Debug.Log("���� ����");
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;

                    // ���� �Ʒ��̰ų�, �����ʿ� ������
                    if (meY > otherY || (meY == otherY && meX > otherX))
                    {
                        // other�� �����
                        other.HideObject();

                        // �����ܰ踦 �����Ѵ�.
                        CraeteObject(meX, meY);
                    }
                }
                else return;

            }
            else return;
        }
        else return;
    }

    private void HideObject()
    {
        // bool�� ���ؼ� �浹 �̺�Ʈ�� 2�� �Ͼ�� �ʵ��� �ϰ�, ����
        checkCollision = true;
        Destroy(gameObject);
    }

    private void CraeteObject(float x, float y)
    {
        // object�� ��ġ�� �����ܰ��� ������ �����ϰ�, ���� ����, ����
        checkCollision = true;
        Vector2 pos = new Vector2(x, y+1f);
        quaternion quaternion = quaternion.identity;
        Instantiate(nextJelly, pos, quaternion);

        gameManager.SetScore(level);

        Destroy(gameObject);
    }

}
