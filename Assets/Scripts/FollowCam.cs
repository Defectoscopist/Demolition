using UnityEngine;
using System.Collections;
using System;
public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // ������ �� ������������ ������ (Point of interest)

    [Header("Set in Inspector")]
    public float easing = 0.05f; // ���������� ��������� ������
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
  public float camZ; // ���������� Z ������

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        /*if (POI == null) return; // �����, ���� ��� �������

        // �������� ������� �������
        Vector3 destination = POI.transform.position;*/

        // ���������� ������
        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            if (POI.tag == "Projectile")
            { // ���� ��� ������...
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                { // � ���� �� ����
                    POI = null; // ������� ������
                    return;
                }
            }
        }

        // ���������� X � Y ������������ ����������
        destination.x = MathF.Max(minXY.x, destination.x);
        destination.y = MathF.Max(minXY.y, destination.y);
        // ��� ��������� ������
        destination = Vector3.Lerp(transform.position, destination, easing);
        // ���������� ������ ��������
        destination.z = camZ;
        transform.position = destination;
        // ����� ����� ���������� � ���� ������
        Camera.main.orthographicSize = destination.y + 10;
    }
}