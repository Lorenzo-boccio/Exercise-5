using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject arrow1;  //fill in by Inspector
    private Arrow1 arrow1_script;
     
    // Start is called before the first frame update
    void Start()
    {
        arrow1_script = arrow1.GetComponent<Arrow1>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            arrow1_script.LiftOff();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            arrow1_script.CeaseThrust();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            arrow1_script.Halt();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            arrow1_script.AdjustThrustAngle(+1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            arrow1_script.AdjustThrustAngle(-1);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin.box.fontSize = 15;
        GUI.skin.box.wordWrap = false;

        GUI.Box(new Rect(5, 115, 150, 30), "speed = " + arrow1_script.GetSpeed());

        GUI.Box(new Rect(5, 145, 150, 30), "angle theta = " + arrow1_script.GetTheta());

        GUI.Box(new Rect(5, 175, 150, 30), "angle lambda = " + arrow1_script.GetLambda());

        GUI.Box(new Rect(5, 205, 150, 30), "time = " + arrow1_script.GetTime());

        GUI.Box(new Rect(5, 235, 150, 30), "distance = " + arrow1_script.GetDistance());

    }

}
