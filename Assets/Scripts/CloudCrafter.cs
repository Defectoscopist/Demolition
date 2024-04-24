using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40; // Число облаков
    public GameObject cloudPrefab; // Шаблон для облаков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; // Мин. масштаб каждого облака
    public float cloudScaleMax = 3; // Макс. масштаб каждого облака
    public float cloudSpeedMult = 0.5f; // Коэф. скорости облаков

    private GameObject[] cloudInstances;

    void Awake()
    {
        // Создаем массив для хранения всех облаков
        cloudInstances = new GameObject[numClouds];
        // Найти родительский CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");

        // Создаем в цикле заданное число облаков
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            // Создаем экземпляр с cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Местоположение облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            // Маленькие облака должны быть ближе к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            // и дальше
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Сделать облако дочерним по отношению к anchor
            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;
        }
    }

    void Update()
    {
        // Варьируем скорость облаков
        foreach (GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Увеличить скорость
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            if (cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }
            cloud.transform.position = cPos;
        }
    }

}