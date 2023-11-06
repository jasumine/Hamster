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

    public float delayTime;
    public bool isdelay = false;
    private int jellyNum;

    private UIManager ui;

    private void Start()
    {
        nowJelly = nowJelly.GetComponent<SpriteRenderer>();
        nextJelly = nextJelly.GetComponent<SpriteRenderer>();
        ui = gameObject.GetComponent<UIManager>();
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

                    Instantiate(jellys[jellyNum], newPos, rotation);

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
            jellyNum = UnityEngine.Random.Range(0, jellys.Length);
            JellyController jelly = jellys[jellyNum].GetComponent<JellyController>();

            nextJelly.sprite = jelly.jellyImage;
        }
        else return;
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

        ui.showScore(score);
    }
}
