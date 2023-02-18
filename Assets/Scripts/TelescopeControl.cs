using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeControl : MonoBehaviour
{
    public GameObject arrow1;
    private Vector3 offset = new Vector3(0, 0, -12);  //necessary to back the camera away in the z-direction to view objects in the z=0 plane

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = arrow1.transform.position + offset ;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = arrow1.transform.position + offset;
    }
}
