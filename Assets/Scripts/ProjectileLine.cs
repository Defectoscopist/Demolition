using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        // Создаем точки
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() // Стереть линию
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint() // Добавление точки в линии
    {
        Vector3 pt = _poi.transform.position;
        // Если точка недостаточно далека от предыдущей
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return; // просто выйти
        }

        /* Это было в учебнике, но выглядит тупо
        if (points.Count == 0)
        { // Если это точка запуска...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            // ... добавить фрагмент линии
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
        }
        else
        { // Или обычная последовательность добавления точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }*/
        points.Add(pt);
        line.positionCount = points.Count;
        line.SetPosition(points.Count - 1, lastPoint);
        line.enabled = true;
    }

    // Местоположение последней добавленной точки
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        { // Если пустой, найти нужный объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        // Если все-таки нашелся, добавить точку с координатами
        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }
}