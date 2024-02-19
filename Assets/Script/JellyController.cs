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
        // overlap은 collider layer 확인
        // bone layer - spring / jelly layer - jelly name
        // collider를 layer 구분으로 2개를 둠
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
        // 내가 아닌 경우
        if (otherJelly != null && otherJelly != this)
        {
            // 젤리이면서, 레벨이 같고
            if (otherJelly.tag == "Jelly" && otherJelly.level == level)
            {
                // Debug.Log(gameObject.name + "젤리 레벨이 같다.");
                {
                    // Debug.Log(gameObject.name + "젤리 콜라이더 체크중.");
                    float meX = boneCollider.bounds.center.x;
                    float meY = boneCollider.bounds.center.y;
                    float otherX = otherJelly.boneCollider.bounds.center.x;
                    float otherY = otherJelly.boneCollider.bounds.center.y;

                    // Debug.Log(gameObject.name + "젤리 콜라이더 체크 완료.");
                    // Debug.Log(gameObject.name+boneCollider.bounds.center + otherJelly.boneCollider.bounds.center);

                    // Debug.Log(gameObject.name + boneCollider.gameObject.name + "젤리 숨김 체크 중.");

                    // 최근에 만들어 진 것을 기준으로(num이 높음)
                    if (objectNumber < otherJelly.objectNumber)
                     {
                        // delayCreate가 딜레이 중이 아니라면 (false)
                        if(GameManager.GetInstance().isDelayCreate== false)
                        {
                            // Debug.Log(gameObject.name+ boneCollider.gameObject.name + "젤리 숨김 체크 중.");
                            // other을 숨기고
                            otherJelly.HideObject();

                            // 다음단계를 생선한다.
                            CraeteObject(meX, meY);
                            // Debug.Log(gameObject.name + "젤리 합쳐짐.");
                            GameManager.GetInstance().isDelayCreate =true;
                        }
                    }
                    /*
                    else
                    {
                        Debug.Log(gameObject.name + objectNumber+"뭔가 안댐");
                        // 충돌했는데 합쳐지지 않는경우 -> 중점에서부터 서로의 반지름의 길이 보다 짧은경우-> 지름의 길이보다 짧은경우 -> 합쳐지도록 한다. (원의 충돌체크)

                        // me의 중점부터 other의 중점까지의 길이 < overlap의 radius *2 거리 라면 합친다.
                        Debug.Log(gameObject.name + objectNumber + boneCollider.bounds.center + ", other : " + otherJelly.name + otherJelly.objectNumber + otherJelly.boneCollider.bounds.center);


                        float distance = Vector3.Distance(boneCollider.bounds.center, otherJelly.boneCollider.bounds.center);

                        float overlapRadius2 = radius * 2;

                        if (distance <= overlapRadius2)
                        {
                            Debug.Log(gameObject.name + objectNumber + "합치기!!!1");
                            // 번호가 더 높다 = 최근에 생성 됬다.
                            if (objectNumber < otherJelly.objectNumber)
                            //(meY > otherY || meX > otherX)
                            // (meY > otherY || (meY == otherY && meX > otherX))
                            {
                                Debug.Log(gameObject.name + boneCollider.gameObject.name + "젤리 숨김 체크 중.");
                                // other을 숨기고
                                otherJelly.HideObject();

                                // 다음단계를 생선한다.
                                CraeteObject(meX, meY);
                                Debug.Log(gameObject.name + objectNumber + "젤리 합쳐짐.");
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
        // 충돌 이벤트가 2번 일어나지 않도록 숨긴다.
        this.gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    private void CraeteObject(float x, float y)
    {
        // object의 위치에 다음단계의 젤리를 생성하고, 점수 갱신, 삭제
        Vector2 pos = new Vector2(x, y);
        quaternion quaternion = quaternion.identity;

        // 레벨이 9라면 마지막 단계인 다람쥐를 생성하고, 아닐 경우 합쳐준다.
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

    // 게임 오버시에 젤리들의 이미지를 바꾸어 준다.
    private void SetEndImage()
    {
        if(GameManager.GetInstance().isEnd==true)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = jellyImageEnd;

        }
    }

    // 오디오, 이펙트 실행 -> 오브젝트 생성, 점수 추가 -> 이펙트를 끄고 기존의 오브젝트를 숨긴다. 
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


    // overlap 범위를 눈으로 확인하기 위함
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boneCollider.bounds.center, radius);
    }
}
