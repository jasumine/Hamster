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

    [SerializeField] private CircleCollider2D boneCollider;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {

       CheckOverlap();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{ 
    //    if(collision.gameObject.tag =="Jelly")
    //    {
    //        JellyController other = collision.gameObject.GetComponent<JellyController>();
    //        if (other.level == level)
    //        {

    //            if (!checkCollision)
    //            {
    //                Debug.Log("���� ����");
    //                float meX = transform.position.x;
    //                float meY = transform.position.y;
    //                float otherX = other.transform.position.x;
    //                float otherY = other.transform.position.y;

    //                // ���� �Ʒ��̰ų�, �����ʿ� ������
    //                if (meY > otherY || (meY == otherY && meX > otherX))
    //                {
    //                    // other�� �����
    //                    other.HideObject();

    //                    // �����ܰ踦 �����Ѵ�.
    //                    CraeteObject(meX, meY);
    //                }
    //            }
    //            else return;

    //        }
    //        else return;
    //    }
    //    else return;
    //}

     private void CheckOverlap()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(boneCollider.bounds.center, radius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
           // Debug.Log(colliders[i].name);

            JellyController otherJelly = colliders[i].gameObject.GetComponent<JellyController>();
            if (otherJelly != null && otherJelly!= this)
            {
                if (otherJelly.tag == "Jelly")
                {
                    if (otherJelly.level == level)
                    {

                        if (!checkCollision)
                        {
                            //Debug.Log("���� ����");
                            float meX = boneCollider.bounds.center.x;
                            float meY = boneCollider.bounds.center.y;
                            float otherX = otherJelly.boneCollider.bounds.center.x;
                            float otherY = otherJelly.boneCollider.bounds.center.y;

                            // ���� �Ʒ��̰ų�, �����ʿ� ������
                            if (meY > otherY || (meY == otherY && meX > otherX))
                            {
                                // other�� �����
                                otherJelly.HideObject();

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
            else
            {
                //Debug.Log("�浹�Ǵ� ������ �����ϴ�.");
                return;
            }
        }
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
        Vector2 pos = new Vector2(x, y + 1f);
        quaternion quaternion = quaternion.identity;
        Instantiate(nextJelly, pos, quaternion);

       // gameManager.SetScore(level);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boneCollider.bounds.center, radius);
    }
}
