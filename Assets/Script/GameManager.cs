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

    // jelly list를 만들어서 list는 for문을 돌려서-> setActive(true)일 경우 overlap을 호출한다.
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
                // 마우스 버튼을 눌렀다면
                //if (Input.GetMouseButtonUp(0) && ui.isShake==false)
                //{
                //    if (!EventSystem.current.IsPointerOverGameObject())
                //    {
                //        //클릭 처리
                //        // 마우스의 x좌표를 받아와서 player의 위치를 x로 옮기고 push
                //        MousePosition = Input.mousePosition;
                //        MousePosition = camera.ScreenToWorldPoint(MousePosition);


                //        float mouseX = MousePosition.x;
                //        player.gameObject.transform.position = new Vector2(mouseX, playerYPos);


                //        DropJelly();
                //    }
                //}

                // 터치를 했다면
                if (Input.touchCount > 0 && ui.isShake == false && ui.isCanTouch == true)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (!EventSystem.current
                   .IsPointerOverGameObject(touch.fingerId))
                        {
                            //터치 처리

                            // 터치의 x좌표를 받아와서 player의 위치를 x로 옮기고 push

                            //MousePosition = touch.position;
                            //MousePosition = camera.ScreenToWorldPoint(MousePosition);

                            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                            float mouseX = touchPosition.x;
                            player.gameObject.transform.position = new Vector2(mouseX, playerYPos);


                            DropJelly();
                        }
                    }
                }

                // 스페이스바를 눌렀다면
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
        // player의 x좌표에 push
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
                    // 내 점수가 3등보다 높을 경우(2등과 같을 경우 포함)
                    // 새로운 점수를 넣는다.
                    PlayerPrefs.SetInt("ThirdScore", score);
                    PlayerPrefs.Save();
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

    public void ReStartGame()
    {
        // 오디오를 끄고,
        // 점수를 저장
        audioManager.SetAudio("EXIT");
        Save();

        // 젤리 초기화 및 삭제
        nowJelly.sprite = null;
        nextJelly.sprite = null;

        jellyObjects.Clear();

        InitJelly();

        foreach (Transform child in jellyBundle.transform)
        {
            Destroy(child.gameObject);
        }

        // player 위치 초기화
        player.gameObject.transform.position = new Vector2(0, playerYPos);


        // PopUp off
        ui.UnActiveSettingPopUp();
        if(isEnd==true)
        {
            ui.UnActiveGameOverImage();
            isEnd = false;
            Time.timeScale = 1;
        }

        // 점수 초기화 및 점수팡 갱신, 브금 실행
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
