using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //������� ������ ��������
    [Header("Set in Inspector")]
    public Text uitLevel; // ������ �� ������ �� UIText_Level
    public Text uitShots; // ������ �� ������ �� UIText_Shots
    public Text uitButton; // ������ �� �������� ������ �� Text � UIButton_View

    public GameObject uitCongrats;
    public GameObject uitPlease;


    public Vector3 castlePos;
    public GameObject[] castles;
    public string congrats;
    public string hire;
    static public bool finish = false;
    
    [Header("Set Dynamically")]
    public int level; // ����� ������
    public int levelMax; // ���-�� �������
    public int shotsTaken; // ��������� �������
    public GameObject castle; // �������� �����
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // ����� FollowCam

    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }


    public void StartLevel()
    {
        if (castle != null)
        { // ���� ���� �����
            Destroy(castle); // ����������
        }

        // ������� �� ��������
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // ������� �����
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // ������ �� �������
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // ������ �� ����������������� ����������
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        //��������� ���������� ������
        if ((mode == GameMode.playing) && Goal.goalMet == true)
        { //������ ==true ���� �� ���������
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    public void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {

            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        // ���� ��������
        S.shotsTaken++;
    }
}
