using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public CircleCollider2D[] boneObject;
    public CircleCollider2D[] bodyObject;

    private void Update()
    {
        UpdatePos();
    }

    void UpdatePos()
    {
        for(int i = 0; i< boneObject.Length; i++)
        {
            float x = boneObject[i].bounds.center.x;
            float y = boneObject[i].bounds.center.y;

            Vector2 bodyPos = bodyObject[i].bounds.center;
            bodyPos.x = x;
            bodyPos.y = y;

            bodyObject[i].offset = bodyObject[i].transform.InverseTransformPoint(bodyPos);
        }

    }

}
