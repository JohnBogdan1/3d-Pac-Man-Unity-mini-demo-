using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthAnimationVertical : MonoBehaviour {
    public GameObject mainBody;
    private Vector3 offset;

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

        if (horizontalDiff != 0)
        {
            m.enabled = true;
            transform.position = mainBody.transform.position - new Vector3(
                offset.x * ((horizontalDiff >= 0.0f) ? 1 : -1),
                offset.y,
                offset.z
                );
        }
        if (verticalDiff != 0)
            m.enabled = false;
        // vertical - sus/jos
        // horizontal - stanga/dreapta
    }
}
