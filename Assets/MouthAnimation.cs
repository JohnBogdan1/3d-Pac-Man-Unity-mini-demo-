using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class MouthAnimation : MonoBehaviour {
    public GameObject mainBody;
    private Vector3 offset;
    private double lastTimeMeasurement;
    private Stopwatch watch;
    private int sign;
    private bool hasStarted;
    private const long timeInterval = 300;

    public MouthAnimation()
    {
        watch = new Stopwatch();
        sign = 1;
        hasStarted = false;
    }

    //default x scaling: -1.363567
    //default y scaling: 0.9689839
    //default z scaling: -0.7540989

    //max x sc:-1.923579
    //max y sc:1.772625
    //max z sc:-0.7540989
    void Start()
    {
        offset = mainBody.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float verticalDiff = Input.GetAxis("Vertical");
        float horizontalDiff = Input.GetAxis("Horizontal");
        MeshRenderer m = gameObject.GetComponent<MeshRenderer>();
        if( hasStarted )
            System.Console.WriteLine(watch.ElapsedMilliseconds);

        if (verticalDiff != 0)
        {            
            m.enabled = true;
            transform.position = mainBody.transform.position - new Vector3(
                offset.x,
                offset.y,
                offset.z * ((verticalDiff >= 0.0f) ? -1 : 1)
                );

            if ( !hasStarted)
            {
                hasStarted = true;
                watch.Start();
            }
            else
            {
                if( watch.ElapsedMilliseconds >= timeInterval )
                {
                    System.Console.WriteLine(watch.ElapsedMilliseconds);
                    transform.localScale += sign * Time.deltaTime * new Vector3(0.2f, 0.0f, 0.2f);
                    watch.Reset();
                }
                if( transform.localScale.y > 1.772625 || transform.localScale.y < 0.9689839 )
                {
                    sign *= -1;
                }
            }

        }
        if (horizontalDiff != 0)
            m.enabled = false;
        // vertical - sus/jos
        // horizontal - stanga/dreapta
    }
}
