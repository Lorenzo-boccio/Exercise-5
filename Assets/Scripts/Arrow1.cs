using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow1 : MonoBehaviour
{

    /*

    Twilight Zone Episode 15, opening narration: 

    Her name is the Arrow 1. She represents four and a half years of planning, preparation, and training, 
    and a thousand years of science, mathematics, 
    and the projected dreams and hopes of not only a nation, but a world. 

    She is the first manned aircraft into space and this is the countdown. 
    The last five seconds before man shot an arrow into the air. 

    https://en.wikipedia.org/wiki/I_Shot_an_Arrow_into_the_Air    

    */

    private int lambda;
    const float g = 9.8f;
    const float mltpl = 1.2f; //factor to multiply g by to obtain ||thrust||
    private Vector3 thrust;
    private Vector3 gravity; //constant in magnitude and direction
    private Vector3 acc;
    private Vector3 vel;
    private Vector3 pos;
    private bool moving;
    private GameObject trail;
    public LineRenderer plotRenderer;  //fill in the Inspector
    private Plotter plotter; //the script component of lineRenderer

    private float speed;
    private int theta;
    private float distance;
    private double elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        gravity = new Vector3(0, -g, 0);
        lambda = 90; //lambda is the angle that the thrust vector makes with the horizontal
        thrust = mltpl * g * new Vector3(Mathf.Cos(lambda * Mathf.Deg2Rad), Mathf.Sin(lambda * Mathf.Deg2Rad), 0);

        plotter = plotRenderer.GetComponent<Plotter>();

        moving = false;

        acc = Vector3.zero;
        vel = Vector3.zero; //velocity, initialized at time 0 using initial speed v and direction lambda

        pos = Vector3.zero;
        transform.position = pos;

        trail = transform.GetChild(0).gameObject;  //this is a requirement, so they understand how the transform component manages parent/child relationships

        speed = 0f;
        theta = 90;
        distance = 0f;
        elapsedTime = 0f;
    }

    public void AdjustThrustAngle(int delta_Lambda)
    {

        lambda += delta_Lambda;
        if (lambda > 90)
            lambda -= 1;
        if (lambda < 45)
            lambda += 1;

        thrust = mltpl * g * new Vector3(Mathf.Cos(lambda * Mathf.Deg2Rad), Mathf.Sin(lambda * Mathf.Deg2Rad), 0);
        acc = gravity + thrust;
    }

    public void LiftOff()
    {
        acc = gravity + thrust;
        moving = true;
    }

    public void CeaseThrust()
    {
        acc = gravity;
        trail.GetComponent<TrailRenderer>().emitting = false;  //because this journey was just getting to be too exhausting
        PlotTrajectory(pos, vel);
    }

    public void Halt()
    {
        acc = Vector3.zero;
        vel = Vector3.zero;
        moving = false;
    }

    public int GetSpeed()
    {
        return (int)speed;
    }

    public int GetTheta()
    {
        return theta;
    }

    public int GetLambda()
    {
        return lambda;
    }

    public int GetTime()
    {
        return (int)elapsedTime;
    }

    public int GetDistance()
    {
        return (int)distance;
    }

   

    // Update is called once per frame;
    void Update()
    {
        if (moving)
        {
            //step-by-step euler integration method, based on a linear approximation
            //NOTE:  acc includes acceleration due to force of gravity and (until it stops firing) the thrust of the rocket's engine
            vel = vel + Time.deltaTime * acc;
            pos = pos + Time.deltaTime * vel;
            transform.position = pos;

            if (vel.sqrMagnitude > 0)
            {
                //use this, or the alternative shown in P_arrow_bolical Plot
                transform.up = vel.normalized;  //NOTE:  only want to do this if vel is not zero!
            }

            speed = vel.magnitude;
            theta = (int)(Mathf.Rad2Deg * Mathf.Atan2(vel.y, vel.x));  //note the potential for error if parenthesis not used here!
            distance += speed * Time.deltaTime; //an estimate of how far rocket will travel during the next time step
            elapsedTime += Time.deltaTime;
        }
    }

    /*
    public void ShowPlot(bool showPlot)
    {
         plotter.gameObject.SetActive(showPlot);
         if(showPlot)
           PlotTrajectory(pos, vel); //NOTE:  this only shows if SetActive(true)
    }
    */

    public void PlotTrajectory(Vector3 initPos, Vector3 initVel)
    {
        float tT = 0f;
        float timeStep = .2f;
        float x;
        float y;
        float z = 0f;

        plotter.lineRenderer.SetPosition(0, initPos);

        for (int i = 1; i < (plotter.segments + 1); i++)
        {
            tT += timeStep;  //total elapsed time; note that timeStep is in virtual time, not real time
            x = initPos.x + initVel.x * (tT);  //horizontal velocity is not affected by gravity
            y = initPos.y + initVel.y * (tT) - 0.5f * g * (tT) * (tT);
            plotter.lineRenderer.SetPosition(i, new Vector3(x, y, z));
        }
    }
}
