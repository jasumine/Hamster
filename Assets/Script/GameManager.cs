using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gameSceneList;

    [SerializeField] private GameObject[] jellys;
    [SerializeField] private SpriteRenderer nowJelly;
    [SerializeField] private SpriteRenderer nextJelly;

    public ParticleSystem thunderEffect;
    public GameObject glovalVolume;

    [SerializeField] private GameObject gameOverBox;

    public bool ismove = true;
    public int score = 0;

    public float delayTime = 0.5f;
    public bool isdelay = false;
    private int nowJellyNum;
    private int nextJellyNum;

    public bool isEnd = false;
    public TextMeshProUGUI[] rankScoreTexts;



    private UIManager ui;
    private SheetsManager sheetsManager;
    public AudioManager audioManager;

    private void Start()
    {
        Time.timeScale = 1;
        nowJelly = nowJelly.GetComponent<SpriteRenderer>();
        nextJelly = nextJelly.GetComponent<SpriteRenderer>();
        ui = gameObject.GetComponent<UIManager>();
       // sheetsManager = gameObject.GetComponent<SheetsManager>();
        
        InitJelly();

        //PlayerPrefs.SetInt("ThirdScore", 0);
        //PlayerPrefs.SetInt("SecondScore",0);
        //PlayerPrefs.SetInt("FirstScore", 0);

        LoadScorce();

       // StartCoroutine(RankData());
    }

    private void Update()
    {
        PushJelly();
        CheckDeadLine();
    }

    private void PushJelly()
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
                    audioManager.SetAudio("Drop");

                    SetScore(nowJellyNum);

                    nowJelly.sprite = null;
                    SetNowJelly();
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

    private void SetNowJelly()
    {
        if (nowJelly.sprite == null)
        {
            nowJellyNum = nextJellyNum;
            nowJelly.gameObject.transform.localScale = nextJelly.gameObject.transform.localScale;
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

            nextJelly.gameObject.transform.localScale = jellys[nextJellyNum].gameObject.transform.localScale;
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

        ui.ShowScore(score);
    }

    private void CheckDeadLine()
    {
        if(!isdelay && ui.isShake== false)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(gameOverBox.transform.position, gameOverBox.transform.localScale,0);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Jelly")
                {
                    Debug.Log("게임오버");
                    isEnd = true;

                    audioManager.SetAudio("Over");
                    Time.timeScale = 0;
                    ui.EndImage.SetActive(true);
                }
            }
        }
    }


    public void Save()
    {
        Debug.Log(score);

        if (score <= PlayerPrefs.GetInt("FirstScore"))
        {
            if (score <= PlayerPrefs.GetInt("SecondScore"))
            {
                if (score <= PlayerPrefs.GetInt("ThirdScore"))
                {
                    // 내 점수가 3등보다 높을 경우(2등과 같을 경우 포함)
                    // 새로운 점수를 넣는다.
                    PlayerPrefs.SetInt("ThirdScore", score);
                    PlayerPrefs.Save();
                    return;
                }
                else
                {
                    return;
                }

            }
            else
            {
                // 내 점수가 2등보다 높을 경우 (1등과 같을 경우도 포함)
                // 3등엔 2등점수를, 2등엔 새로운 점수
                PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
                PlayerPrefs.SetInt("SecondScore", score);
                PlayerPrefs.Save();
                return;
            }
        }
        else
        {
            // 내 점수가 1등보다 높을 경우
            // 3등엔 2등점수를, 2등엔 1등점수를, 1등엔 새로운 점수
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetInt("SecondScore", PlayerPrefs.GetInt("FirstScore"));
            PlayerPrefs.SetInt("FirstScore", score);
            PlayerPrefs.Save();
            return;
        }



    }


    private void LoadScorce()
    {
        rankScoreTexts[0].text = PlayerPrefs.GetInt("FirstScore").ToString();
        rankScoreTexts[1].text = PlayerPrefs.GetInt("SecondScore").ToString();
        rankScoreTexts[2].text = PlayerPrefs.GetInt("ThirdScore").ToString();
    }





    //private IEnumerator RankData()
    //{
    //    yield return StartCoroutine(sheetsManager.PostData("playerid", "playername", score, 60));

    //    yield return StartCoroutine(sheetsManager.GetData());
    //}


    //private IEnumerator DataLoaditng()
    //{
    //    yield return StartCoroutine(sheetsManager.WriteData("playerid", "playername", score, 60));
    //}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(gameOverBox.transform.position, gameOverBox.transform.localScale);
    }
}
