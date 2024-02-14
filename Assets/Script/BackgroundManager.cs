using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    // ȭ�� �ػ󵵸��� ��� �̹��� ũ�⸦ �����ϱ� ���� Ŭ����

    public SpriteRenderer backgroundImage;

    private void Start()
    {
        ResizeBox();
    }

    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.R) )
        {
            ResizeBox();
        }
    }


    private void ResizeBox()
    {
        float spriteX = backgroundImage.sprite.bounds.size.x;
        float spriteY = backgroundImage.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2f;
        float screenX = screenY/ Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX/spriteX), Mathf.Ceil(screenY/spriteY));

    }
}
