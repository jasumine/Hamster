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
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    private void Update()
    {
        CheckOverlap();
        SetEndImage();
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
        // overlap�� collider layer Ȯ��
        // bone layer - spring
        // collider�� layer �������� 2���� ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boneCollider.bounds.center, radius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
           // Debug.Log(colliders[i].name);

            JellyController otherJelly = colliders[i].gameObject.GetComponent<JellyController>();
            if (otherJelly != null && otherJelly!= this)
            {
                if (otherJelly.tag == "Jelly")
                {
                    Debug.Log(gameObject.name + "������ �浹�ߴ�");
                    if (otherJelly.level == level)
                    {
                        Debug.Log(gameObject.name + "���� ������ ����.");
                        if (!checkCollision)
                        {
                            Debug.Log(gameObject.name + "���� �ݶ��̴� üũ��.");
                            //Debug.Log("���� ����");
                            float meX = boneCollider.bounds.center.x;
                            float meY = boneCollider.bounds.center.y;
                            float otherX = otherJelly.boneCollider.bounds.center.x;
                            float otherY = otherJelly.boneCollider.bounds.center.y;
                            Debug.Log(gameObject.name + "���� �ݶ��̴� üũ �Ϸ�.");
                            Debug.Log(gameObject.name+boneCollider.bounds.center + otherJelly.boneCollider.bounds.center);
                            // ���� �Ʒ��̰ų�, �����ʿ� ������
                                    if (meY > otherY || meX > otherX)
                            //(meY > otherY ||  meX > otherX)
                            // (meY > otherY || (meY == otherY && meX > otherX))
                            {
                                Debug.Log(gameObject.name+ boneCollider.gameObject.name + "���� ���� üũ ��.");
                                // other�� �����
                                otherJelly.HideObject();

                                // �����ܰ踦 �����Ѵ�.
                                CraeteObject(meX, meY);
                                Debug.Log(gameObject.name + "���� ������.");
                                break;
                            }
                                    else
                            {
                                Debug.Log("���� �ȴ�");
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
        Vector2 pos = new Vector2(x, y);
        quaternion quaternion = quaternion.identity;
        if (level==9)
        {
           StartCoroutine(ThunderSquirrel(pos,quaternion));
        }
        else if(nextJelly != null)
        {
            GameObject jellyInstance = Instantiate(nextJelly, pos, quaternion);
            jellyInstance.transform.SetParent(gameManager.jellyBundle.transform);

            gameManager.audioManager.SetAudio("Merge");

            gameManager.SetScore(level);

            Destroy(gameObject);
        }
        else
        {
            gameManager.audioManager.SetAudio("Thunder");
            gameManager.SetScore(level);

            Destroy(gameObject);
        }
    }

    private void SetEndImage()
    {
        if(gameManager.isEnd==true)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = jellyImageEnd;

        }
    }

    IEnumerator ThunderSquirrel(Vector2 _pos, Quaternion _quaternion)
    {
        gameManager.audioManager.SetAudio("Thunder");
        gameManager.glovalVolume.SetActive(true);
        gameManager.thunderEffect.gameObject.SetActive(true);
        gameManager.thunderEffect.Play();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;

        yield return new WaitForSeconds(0.7f);

        GameObject jellyInstance = Instantiate(nextJelly, _pos, _quaternion);
        jellyInstance.transform.SetParent(gameManager.jellyBundle.transform);

        gameManager.SetScore(level);

        gameManager.glovalVolume.SetActive(false);
        gameManager.thunderEffect.gameObject.SetActive(false);  
        Destroy(gameObject);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boneCollider.bounds.center, radius);
    }
}
