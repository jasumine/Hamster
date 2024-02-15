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
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gameSceneList;

    [SerializeField] private GameObject[] jellys;
    [SerializeField] private SpriteRenderer nowJelly;
    [SerializeField] private SpriteRenderer nextJelly;
    public GameObject jellyBundle;

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


    private Vector2 MousePosition;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;
    [SerializeField] private float playerYPos;
    private UIManager ui;
    public AudioManager audioManager;

    private static GameManager instance;
    private GameManager() { }

    // jelly list�� ���� list�� for���� ������-> setActive(true)�� ��� overlap�� ȣ���Ѵ�.
    // 

    public List<GameObject> jellyObjects; 

    public static GameManager GetInstance()
    {
         return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1;
        nowJelly = nowJelly.GetComponent<SpriteRenderer>();
        nextJelly = nextJelly.GetComponent<SpriteRenderer>();
        ui = gameObject.GetComponent<UIManager>();

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

    private void FixedUpdate()
    {
        CheckJelly();
    }

    private void CheckJelly()
    {
        for(int i = 0; i< jellyObjects.Count; i++)
        { 
            if (jellyObjects[i].activeSelf == true)
            {
                JellyController jellyController = jellyObjects[i].GetComponent<JellyController>();
                jellyController.CheckOverlap();
            }
            else
            {
                jellyObjects.RemoveAt(i);
               
            }
        }
    }


    private void PushJelly()
    {
        if(ismove)
        {
            if(!isdelay)
            {
                // ���콺 ��ư�� �����ٸ�
                //if (Input.GetMouseButtonUp(0) && ui.isShake==false)
                //{
                //    if (!EventSystem.current.IsPointerOverGameObject())
                //    {
                //        //Ŭ�� ó��
                //        // ���콺�� x��ǥ�� �޾ƿͼ� player�� ��ġ�� x�� �ű�� push
                //        MousePosition = Input.mousePosition;
                //        MousePosition = camera.ScreenToWorldPoint(MousePosition);


                //        float mouseX = MousePosition.x;
                //        player.gameObject.transform.position = new Vector2(mouseX, playerYPos);


                //        DropJelly();
                //    }
                //}

                // ��ġ�� �ߴٸ�
                if (Input.touchCount > 0 && ui.isShake == false && ui.isCanTouch == true)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (!EventSystem.current
                   .IsPointerOverGameObject(touch.fingerId))
                        {
                            //��ġ ó��

                            // ��ġ�� x��ǥ�� �޾ƿͼ� player�� ��ġ�� x�� �ű�� push

                            //MousePosition = touch.position;
                            //MousePosition = camera.ScreenToWorldPoint(MousePosition);

                            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                            float mouseX = touchPosition.x;
                            player.gameObject.transform.position = new Vector2(mouseX, playerYPos);


                            DropJelly();
                        }
                    }
                }

                // �����̽��ٸ� �����ٸ�
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DropJelly();
                }
            }
        }
    }

    private void DropJelly()
    {
        CoolDownDelay();
        // player�� x��ǥ�� push
        float x = nowJelly.transform.position.x;
        float y = nowJelly.transform.position.y;
        Vector3 newPos = new Vector3(x, y, 0);
        quaternion rotation = quaternion.identity;

        GameObject jellyObject = Instantiate(jellys[nowJellyNum], newPos, rotation);
        jellyObject.transform.SetParent(jellyBundle.transform);
        jellyObjects.Add(jellyObject);


        audioManager.SetAudio("Drop");

        SetScore(nowJellyNum);

        nowJelly.sprite = null;
        SetNowJelly();
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

                    audioManager.BgmStop();
                    //audioManager.SetAudio("Over");
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
                   
                    return;
                }
                else
                {
                    // �� ������ 3��� ���� ���(2��� ���� ��� ����)
                    // ���ο� ������ �ִ´�.
                    PlayerPrefs.SetInt("ThirdScore", score);
                    PlayerPrefs.Save();
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

    public void ReStartGame()
    {
        // ������� ����,
        // ������ ����
        audioManager.SetAudio("EXIT");
        Save();

        // ���� �ʱ�ȭ �� ����
        nowJelly.sprite = null;
        nextJelly.sprite = null;

        jellyObjects.Clear();

        InitJelly();

        foreach (Transform child in jellyBundle.transform)
        {
            Destroy(child.gameObject);
        }

        // player ��ġ �ʱ�ȭ
        player.gameObject.transform.position = new Vector2(0, playerYPos);


        // PopUp off
        ui.UnActiveSettingPopUp();
        if(isEnd==true)
        {
            ui.UnActiveGameOverImage();
            isEnd = false;
            Time.timeScale = 1;
        }

        // ���� �ʱ�ȭ �� ������ ����, ��� ����
        score = 0;
        ui.ShowScore(score);
        LoadScorce();
        audioManager.BgmStart();
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(gameOverBox.transform.position, gameOverBox.transform.localScale);
    }
}
