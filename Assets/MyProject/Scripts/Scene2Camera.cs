using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Camera : MonoBehaviour
{
    public float xMin = 1f;
    public float xMax = 3f;
    public Texture2D HealthIcon;
    public Texture2D RocketsIcon;
    public Texture2D BombsIcon;

    private GUIStyle style = new GUIStyle();
    private string score;

    private int HP;
    private int rocketsAmmo;
    private int bombsAmmo;

    float targetX;

    private void OnGUI()
    {
        for (int i = 0; i < HP; i++)
            GUI.Box(new Rect(10 + i * 30, Screen.height - 40, 30, 30), HealthIcon);

        GUI.Box(new Rect(10, Screen.height - 120, 30, 30), BombsIcon);
        GUI.Label(new Rect(50, Screen.height - 120, 20, 80), "x" + bombsAmmo.ToString(), style);

        GUI.Box(new Rect(10, Screen.height - 80, 60, 30), RocketsIcon);
        GUI.Label(new Rect(80, Screen.height - 80, 20, 80), "x" + rocketsAmmo.ToString(), style);
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        style.fontSize = 30;
        style.normal.textColor = Color.white;
    }

    private void FixedUpdate()
    {
        HP = MyPlayerControl.Player.GetComponent<MyPlayerHealth>().HP;
        rocketsAmmo = MyPlayerControl.Player.rocketsAmmo;
        bombsAmmo = MyPlayerControl.Player.bombsAmmo;
    }

    private void LateUpdate()
    {
        targetX = Mathf.Lerp(transform.position.x, MyPlayerControl.Player.transform.position.x, Time.deltaTime);
        targetX = Mathf.Clamp(targetX, xMin, xMax);
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
