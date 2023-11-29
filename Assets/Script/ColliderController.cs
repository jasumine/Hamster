using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public CircleCollider2D boneObject;
    public CircleCollider2D bodyObject;

    public Vector3 offset;

    private void FixedUpdate()
    {
        UpdatePos();
    }

    void UpdatePos()
    {
        float x = boneObject.bounds.center.x;
        float y = boneObject.bounds.center.y;

        Vector2 bodyPos = bodyObject.bounds.center;
        bodyPos.x = x;
        bodyPos.y = y;

        bodyObject.offset = bodyObject.transform.InverseTransformPoint(bodyPos);
        //bodyObject.transform.position = bodyPos;
    }


    void Test()
    {
        // boneObject�� �߽� ��ġ�� ������
        Vector3 boneCenter = boneObject.bounds.center;

        // offset�� ����
        Vector3 targetPosition = boneCenter + offset;

        // bodyObject�� ���ϴ� ��ġ�� �̵�
        bodyObject.transform.position = targetPosition;

        foreach (Transform child in transform)
        {
            if (child != boneObject.transform && child != bodyObject.transform)
            {
                // �ٸ� �ڽĵ��� ���� ���� �������� ����
                child.localPosition = Quaternion.Inverse(transform.rotation) * (child.position - transform.position);
            }
        }
    }

    void BeforeMethod()
    {
        float x = boneObject.bounds.center.x;
        float y = boneObject.bounds.center.y;

        Vector2 bodyPos = bodyObject.bounds.center;
        bodyPos.x = x;
        bodyPos.y = y;

        bodyObject.offset = bodyObject.transform.InverseTransformPoint(bodyPos);
    }
}

