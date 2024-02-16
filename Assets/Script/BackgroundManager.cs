using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Device;

public class BackgroundManager : MonoBehaviour
{
    // ȭ�� �ػ󵵸��� ��� �̹��� ũ�⸦ �����ϱ� ���� Ŭ����

    public SpriteRenderer backgroundImage;

    private void Start()
    {
        ResizeBackgroundImage();
        ResizeCameraSize();
    }

    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.R) )
        {
            ResizeBackgroundImage();
            ResizeCameraSize();
        }
    }


    private void ResizeBackgroundImage()
    {
        float spriteX = backgroundImage.sprite.bounds.size.x;
        float spriteY = backgroundImage.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2f;
        float screenX = screenY/ UnityEngine.Screen.height * UnityEngine.Screen.width;

        transform.localScale = new Vector2(spriteX, Mathf.Ceil(screenY/spriteY));

    }

    // ������ �´� ���� ���� �ػ󵵷� ���� + ī�޶� ����
    // ���� �ػ󵵺��� ���ΰ� ���� �� ��쿡�� size�� �����ش�.
    // �ʺ� / ���� = �� * wide  = height

    private void ResizeCameraSize()
    {

        // ������ �Ǵ� X�� Y�� ���� ���� R
        int baseX = 1080;
        int baseY = 2340;

        int baseXR = baseX / baseY;
        int baseYR = baseY / baseX;

      //  Debug.Log("���� : " + baseX + ", ���� : " + baseY);

        int screenX = UnityEngine.Screen.width;
        int screenY = UnityEngine.Screen.height;


        /*
         * w/h = r
         * r = ?
         * ah - bh = mh
         * mh/ ah = 1-r
         * 80/800 = 0.1
         * 1-r = 0.9
         */

        int newX = baseX - screenX;
        int newXR = (1 - (newX / baseX));

    //    Debug.Log("���� : " + newXR);
        Debug.Log(screenX * newXR);

        int newY = baseY - screenY;
        int newYR = (1 - (newY / baseY));

     //   Debug.Log("���� : " + newYR);
    //    Debug.Log(screenY * newYR);
        
        if(newX>=0)
        {
            float newSize = 5 + (newXR);
            Camera.main.orthographicSize = newSize;
        }
        else
        {
            float newSize = 5;
            Camera.main.orthographicSize = newSize;
        }

    }


    int GCD(int a, int b)
    {
        // a>b
        if (b == 0) return 0;
        else return GCD(b, a & b);
    }

}
