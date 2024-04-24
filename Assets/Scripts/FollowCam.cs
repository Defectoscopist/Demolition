using UnityEngine;
using System.Collections;
using System;
public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на интересующий объект (Point of interest)

    [Header("Set in Inspector")]
    public float easing = 0.05f; // регулируем плавность камеры
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
  public float camZ; // Координата Z камеры

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        /*if (POI == null) return; // выйти, если нет объекта

        // Получить позицию объекта
        Vector3 destination = POI.transform.position;*/

        // Возвращаем камеру
        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            if (POI.tag == "Projectile")
            { // Если это снаряд...
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                { // и если он спит
                    POI = null; // вернуть камеру
                    return;
                }
            }
        }

        // Ограничить X и Y минимальными значениями
        destination.x = MathF.Max(minXY.x, destination.x);
        destination.y = MathF.Max(minXY.y, destination.y);
        // Для плавности камеры
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Отодвинуть камеру подальше
        destination.z = camZ;
        transform.position = destination;
        // чтобы земля оставалась в поле зрения
        Camera.main.orthographicSize = destination.y + 10;
    }
}