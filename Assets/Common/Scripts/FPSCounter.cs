using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5F;

    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    private float fps;
    private GUIStyle style;

    void Start()
    {
        if (style == null)
        {
            style = new GUIStyle();
            style.fontSize = 25;
            style.normal.textColor = Color.white;
        }

        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            fps = accum / frames;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 250, 100), "FPS: " + fps.ToString("f2"), style);
    }
}
