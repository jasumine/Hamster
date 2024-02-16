using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Device;

public class BackgroundManager : MonoBehaviour
{
    // 화면 해상도마다 배경 이미지 크기를 조절하기 위한 클래스

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

    // 비율에 맞는 제일 낮은 해상도로 변경 + 카메라 조정
    // 기존 해상도보다 가로가 좁아 질 경우에만 size를 높여준다.
    // 너비 / 높이 = 값 * wide  = height

    private void ResizeCameraSize()
    {

        // 기준이 되는 X와 Y를 나눈 비율 R
        int baseX = 1080;
        int baseY = 2340;

        int baseXR = baseX / baseY;
        int baseYR = baseY / baseX;

      //  Debug.Log("가로 : " + baseX + ", 세로 : " + baseY);

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

    //    Debug.Log("비율 : " + newXR);
        Debug.Log(screenX * newXR);

        int newY = baseY - screenY;
        int newYR = (1 - (newY / baseY));

     //   Debug.Log("비율 : " + newYR);
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
