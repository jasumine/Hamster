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
    public Sprite jellyImageEnd;

    public int level;
    private bool checkCollision = false;

    [SerializeField] private CircleCollider2D boneCollider;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;

    private static int objectCount = 0;
    public int objectNumber;


    private void Start()
    {
        objectCount++;
        objectNumber = objectCount;
    }

    private void Update()
    {
       // CheckOverlap();
        SetEndImage();
    }

    public void CheckOverlap()
    {
        // overlap�� collider layer Ȯ��
        // bone layer - spring / jelly layer - jelly name
        // collider�� layer �������� 2���� ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boneCollider.bounds.center, radius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
           // Debug.Log(colliders[i].name);

            JellyController otherJelly = colliders[i].gameObject.GetComponent<JellyController>();

            otherJellyContoroller(otherJelly);
        }
    }

    private void otherJellyContoroller(JellyController otherJelly)
    {
        // ���� �ƴ� ���
        if (otherJelly != null && otherJelly != this)
        {
            // �����̸鼭, ������ ����
            if (otherJelly.tag == "Jelly" && otherJelly.level == level)
            {
                // Debug.Log(gameObject.name + "���� ������ ����.");
                {
                    // Debug.Log(gameObject.name + "���� �ݶ��̴� üũ��.");
                    float meX = boneCollider.bounds.center.x;
                    float meY = boneCollider.bounds.center.y;
                    float otherX = otherJelly.boneCollider.bounds.center.x;
                    float otherY = otherJelly.boneCollider.bounds.center.y;

                    // Debug.Log(gameObject.name + "���� �ݶ��̴� üũ �Ϸ�.");
                    // Debug.Log(gameObject.name+boneCollider.bounds.center + otherJelly.boneCollider.bounds.center);

                    // Debug.Log(gameObject.name + boneCollider.gameObject.name + "���� ���� üũ ��.");

                    // �ֱٿ� ����� �� ���� ��������(num�� ����)
                    if (objectNumber < otherJelly.objectNumber)
                     {
                        // delayCreate�� ������ ���� �ƴ϶�� (false)
                        if(GameManager.GetInstance().isDelayCreate== false)
                        {
                            // Debug.Log(gameObject.name+ boneCollider.gameObject.name + "���� ���� üũ ��.");
                            // other�� �����
                            otherJelly.HideObject();

                            // �����ܰ踦 �����Ѵ�.
                            CraeteObject(meX, meY);
                            // Debug.Log(gameObject.name + "���� ������.");
                            GameManager.GetInstance().isDelayCreate =true;
                        }
                    }
                    /*
                    else
                    {
                        Debug.Log(gameObject.name + objectNumber+"���� �ȴ�");
                        // �浹�ߴµ� �������� �ʴ°�� -> ������������ ������ �������� ���� ���� ª�����-> ������ ���̺��� ª����� -> ���������� �Ѵ�. (���� �浹üũ)

                        // me�� �������� other�� ���������� ���� < overlap�� radius *2 �Ÿ� ��� ��ģ��.
                        Debug.Log(gameObject.name + objectNumber + boneCollider.bounds.center + ", other : " + otherJelly.name + otherJelly.objectNumber + otherJelly.boneCollider.bounds.center);


                        float distance = Vector3.Distance(boneCollider.bounds.center, otherJelly.boneCollider.bounds.center);

                        float overlapRadius2 = radius * 2;

                        if (distance <= overlapRadius2)
                        {
                            Debug.Log(gameObject.name + objectNumber + "��ġ��!!!1");
                            // ��ȣ�� �� ���� = �ֱٿ� ���� ���.
                            if (objectNumber < otherJelly.objectNumber)
                            //(meY > otherY || meX > otherX)
                            // (meY > otherY || (meY == otherY && meX > otherX))
                            {
                                Debug.Log(gameObject.name + boneCollider.gameObject.name + "���� ���� üũ ��.");
                                // other�� �����
                                otherJelly.HideObject();

                                // �����ܰ踦 �����Ѵ�.
                                CraeteObject(meX, meY);
                                Debug.Log(gameObject.name + objectNumber + "���� ������.");
                            }

                        }
                    }*/
                }
                //else
                //{
                   // Debug.Log(gameObject.name + objectNumber + "checkCollision else");
               // }
            }
           //else
            //{
                //Debug.Log(gameObject.name + objectNumber + "tag level else");
            //}
        }
        //else
       // {
           // Debug.Log(gameObject.name + objectNumber + "null, this ==false  else");
      //  }
    }




    private void HideObject()
    {
        // �浹 �̺�Ʈ�� 2�� �Ͼ�� �ʵ��� �����.
        this.gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    private void CraeteObject(float x, float y)
    {
        // object�� ��ġ�� �����ܰ��� ������ �����ϰ�, ���� ����, ����
        Vector2 pos = new Vector2(x, y);
        quaternion quaternion = quaternion.identity;

        // ������ 9��� ������ �ܰ��� �ٶ��㸦 �����ϰ�, �ƴ� ��� �����ش�.
        if (level==9)
        {
           StartCoroutine(ThunderSquirrel(pos,quaternion));
        }
        else if(nextJelly != null)
        {
            GameObject jellyInstance = Instantiate(nextJelly, pos, quaternion);
            if(jellyInstance != null)
            {
                 
                jellyInstance.transform.SetParent(GameManager.GetInstance().jellyBundle.transform);
                GameManager.GetInstance().jellyObjects.Add(jellyInstance);

                GameManager.GetInstance().audioManager.SetAudio("Merge");

                GameManager.GetInstance().SetScore(level);

                this.gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
        else
        {
            //GameManager.GetInstance().audioManager.SetAudio("Thunder");
            //GameManager.GetInstance().SetScore(level);

            //this.gameObject.SetActive(false);
            //Debug.Log(gameObject.name + objectNumber + "create else");
            //Destroy(gameObject);
        }
    }

    // ���� �����ÿ� �������� �̹����� �ٲپ� �ش�.
    private void SetEndImage()
    {
        if(GameManager.GetInstance().isEnd==true)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = jellyImageEnd;

        }
    }

    // �����, ����Ʈ ���� -> ������Ʈ ����, ���� �߰� -> ����Ʈ�� ���� ������ ������Ʈ�� �����. 
    IEnumerator ThunderSquirrel(Vector2 _pos, Quaternion _quaternion)
    {
        GameManager.GetInstance().audioManager.SetAudio("Thunder");
        GameManager.GetInstance().glovalVolume.SetActive(true);
        GameManager.GetInstance().thunderEffect.gameObject.SetActive(true);
        GameManager.GetInstance().thunderEffect.Play();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;

        yield return new WaitForSeconds(0.7f);

        GameObject jellyInstance = Instantiate(nextJelly, _pos, _quaternion);
        jellyInstance.transform.SetParent(GameManager.GetInstance().jellyBundle.transform);

        GameManager.GetInstance().jellyObjects.Add(jellyInstance);

        GameManager.GetInstance().SetScore(level);

        GameManager.GetInstance().glovalVolume.SetActive(false);
        GameManager.GetInstance().thunderEffect.gameObject.SetActive(false);  
        this.gameObject.SetActive(false);
        // Destroy(gameObject);
    }


    // overlap ������ ������ Ȯ���ϱ� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boneCollider.bounds.center, radius);
    }
}
