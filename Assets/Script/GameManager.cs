using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] jellys;
    [SerializeField] private SpriteRenderer nowJelly;
    [SerializeField] private SpriteRenderer nextJelly;

    public bool ismove = true;
    public int score = 0;

    public float delayTime = 0.5f;
    public bool isdelay = false;
    private int nowJellyNum;
    private int nextJellyNum;

    private UIManager ui;

    private void Start()
    {
        nowJelly = nowJelly.GetComponent<SpriteRenderer>();
        nextJelly = nextJelly.GetComponent<SpriteRenderer>();
        ui = gameObject.GetComponent<UIManager>();
        InitJelly();
    }

    private void Update()
    {
        pushJelly();
    }

    private void pushJelly()
    {
        if(ismove)
        {
            if(!isdelay)
            {
                // 마우스 버튼을 눌렀다면
                if (Input.GetMouseButtonDown(0))
                {
                    // 마우스의 x좌표를 받아와서 player의 위치를 x로 옮기고 push
                }
                // 스페이스바를 눌렀다면
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CoolDownDelay();
                    // player의 x좌표에 push
                    float x = nowJelly.transform.position.x;
                    float y = nowJelly.transform.position.y;
                    Vector3 newPos = new Vector3(x, y, 0);
                    quaternion rotation = quaternion.identity;

                    Instantiate(jellys[nowJellyNum], newPos, rotation);

                    nowJelly.sprite = null;
                    setNowJelly();
                }
            }
        }
    }

    private void CoolDownDelay()
    {
        isdelay = true;

        Invoke("DelayFalse", delayTime);
    }
    private void DelayFalse()
    {
        isdelay = false;
    }

    private void setNowJelly()
    {
        if (nowJelly.sprite == null)
        {
            nowJellyNum = nextJellyNum;
            nowJelly.sprite = nextJelly.sprite;
            nextJelly.sprite = null;
            LandNextJelly();
        }
        else return;

    }

    private void LandNextJelly()
    {
        if (nextJelly.sprite == null)
        {
            nextJellyNum = UnityEngine.Random.Range(0, jellys.Length);
            JellyController jelly = jellys[nextJellyNum].GetComponent<JellyController>();

            nextJelly.sprite = jelly.jellyImage;
        }
        else return;
    }


    private void InitJelly()
    {
        LandNextJelly();
        nowJellyNum = nextJellyNum;
        nowJelly.sprite = nextJelly.sprite;
        nextJelly.sprite = null;
        LandNextJelly();
    }


    public void SetScore(int level)
    {
        // 계차수열을 방식으로 레벨에 따라 점수를 다르게 더해준다
        int sum = 0;
        for (int i = 0; i <= level; i++)
        {
            if (level <= i)
                sum = (sum + i + 1);
        }

        // score에 점수를 더해준다.
        score += sum;

        ui.showScore(score);
    }
}
