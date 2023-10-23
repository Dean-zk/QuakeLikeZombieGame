using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSysVars : MonoBehaviour
{
    QMovement qPlr;
    public GUIStyle style;

    public float fpsDisplayRate = 4.0f; // 4 updates per sec

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    public float playerTopVelocity = 0.0f;

    private int hp = 100;

    void Start()
    {
        qPlr = GetComponent<QMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        FpsCalc();
        /* Calculate top velocity */
        TopVelocity();
    }

    float FpsCalc()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / fpsDisplayRate)
        {
            fps = Mathf.Round(frameCount / dt);
            frameCount = 0;
            dt -= 1.0f / fpsDisplayRate;
        }

        return dt;
    }

    float TopVelocity()
    {
        Vector3 udp = qPlr.playerVelocity;
        udp.y = 0.0f;
        if (udp.magnitude > playerTopVelocity)
        { playerTopVelocity = udp.magnitude; }

        return playerTopVelocity;
    }
    

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 100), "FPS: " + fps, style);
        Vector3 ups = qPlr.body.velocity; //! 2 NULL REF
        ups.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups", style);
        GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "ups", style);
        GUI.Label(new Rect(0, 45, 400, 100), "Health: " + hp, style);
    }
}
