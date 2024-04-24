using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    // поля, устанавливаемы в Inspector'е Unity
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f; // ускорение шарика

    // поля, устанавливаемые динамически
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile; // снаряд
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint"); // найти дочерний объект "LaunchPoint"
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // игнорируем такие объекты как launchPoint
        launchPos = launchPointTrans.position; // устанавливаем координаты запуска
    }

    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // Игрок нажал кнопку мыши, когда указатель на рогатке
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate(prefabProjectile) as GameObject;
        // поместить в launchPoint
        projectile.transform.position = launchPos;
        // сделать его кинематическим
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        // Если рогатка не в режиме прицеливания, ничего не сделать
        if (!aimingMode) return;

        // получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        { // если мышь вышла за пределы сфера
            mouseDelta.Normalize(); // делаем длину вектора равной 1
            mouseDelta *= maxMagnitude; // и сохраняем направление
        }

        // Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {// Кнопка мыши отжата
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult; // запускаем шарик
            FollowCam.POI = projectile; // камера следит за шариком
            projectile = null;
        }
        // страница 551, можно потестировать
    }

}