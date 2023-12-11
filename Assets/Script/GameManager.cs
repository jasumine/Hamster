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
                // ���콺 ��ư�� �����ٸ�
                if (Input.GetMouseButtonDown(0))
                {
                    // ���콺�� x��ǥ�� �޾ƿͼ� player�� ��ġ�� x�� �ű�� push
                }
                // �����̽��ٸ� �����ٸ�
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CoolDownDelay();
                    // player�� x��ǥ�� push
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
        // ���������� ������� ������ ���� ������ �ٸ��� �����ش�
        int sum = 0;
        for (int i = 0; i <= level; i++)
        {
            if (level <= i)
                sum = (sum + i + 1);
        }

        // score�� ������ �����ش�.
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
                    Debug.Log("���ӿ���");
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
                    // �� ������ 3��� ���� ���(2��� ���� ��� ����)
                    // ���ο� ������ �ִ´�.
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
                // �� ������ 2��� ���� ��� (1��� ���� ��쵵 ����)
                // 3� 2��������, 2� ���ο� ����
                PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
                PlayerPrefs.SetInt("SecondScore", score);
                PlayerPrefs.Save();
                return;
            }
        }
        else
        {
            // �� ������ 1��� ���� ���
            // 3� 2��������, 2� 1��������, 1� ���ο� ����
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
