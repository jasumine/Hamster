using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class JellyController : MonoBehaviour
{
    // 다음 단계의 젤리
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
    //                Debug.Log("같은 레벨");
    //                float meX = transform.position.x;
    //                float meY = transform.position.y;
    //                float otherX = other.transform.position.x;
    //                float otherY = other.transform.position.y;

    //                // 내가 아래이거나, 오른쪽에 있을때
    //                if (meY > otherY || (meY == otherY && meX > otherX))
    //                {
    //                    // other을 숨기고
    //                    other.HideObject();

    //                    // 다음단계를 생선한다.
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
        // overlap은 collider layer 확인
        // bone layer - spring
        // collider를 layer 구분으로 2개를 둠
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boneCollider.bounds.center, radius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
           // Debug.Log(colliders[i].name);

            JellyController otherJelly = colliders[i].gameObject.GetComponent<JellyController>();
            if (otherJelly != null && otherJelly!= this)
            {
                if (otherJelly.tag == "Jelly")
                {
                    Debug.Log(gameObject.name + "젤리가 충돌했다");
                    if (otherJelly.level == level)
                    {
                        Debug.Log(gameObject.name + "젤리 레벨이 같다.");
                        if (!checkCollision)
                        {
                            Debug.Log(gameObject.name + "젤리 콜라이더 체크중.");
                            //Debug.Log("같은 레벨");
                            float meX = boneCollider.bounds.center.x;
                            float meY = boneCollider.bounds.center.y;
                            float otherX = otherJelly.boneCollider.bounds.center.x;
                            float otherY = otherJelly.boneCollider.bounds.center.y;
                            Debug.Log(gameObject.name + "젤리 콜라이더 체크 완료.");
                            Debug.Log(gameObject.name+boneCollider.bounds.center + otherJelly.boneCollider.bounds.center);
                            // 내가 아래이거나, 오른쪽에 있을때
                                    if (meY > otherY || meX > otherX)
                            //(meY > otherY ||  meX > otherX)
                            // (meY > otherY || (meY == otherY && meX > otherX))
                            {
                                Debug.Log(gameObject.name+ boneCollider.gameObject.name + "젤리 숨김 체크 중.");
                                // other을 숨기고
                                otherJelly.HideObject();

                                // 다음단계를 생선한다.
                                CraeteObject(meX, meY);
                                Debug.Log(gameObject.name + "젤리 합쳐짐.");
                                break;
                            }
                                    else
                            {
                                Debug.Log("뭔가 안댐");
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
                //Debug.Log("충돌되는 젤리가 없습니다.");
                return;
            }
        }
    }

    private void HideObject()
    {
        // bool을 통해서 충돌 이벤트가 2번 일어나지 않도록 하고, 삭제
        checkCollision = true;
        Destroy(gameObject);
    }

    private void CraeteObject(float x, float y)
    {
        // object의 위치에 다음단계의 젤리를 생성하고, 점수 갱신, 삭제
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
