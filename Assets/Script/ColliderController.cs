using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public CircleCollider2D boneObject;
    public CircleCollider2D bodyObject;


    private void Update()
    {
        UpdatePos();
    }

    // bone의 중심위치에 body를 맞춘다.
    void UpdatePos()
    {
        float x = boneObject.bounds.center.x;
        float y = boneObject.bounds.center.y;

        Vector2 bodyPos = bodyObject.bounds.center;
        bodyPos.x = x;
        bodyPos.y = y;

        bodyObject.offset = bodyObject.transform.InverseTransformPoint(bodyPos);
    }
}

