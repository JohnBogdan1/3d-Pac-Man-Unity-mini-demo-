using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMove : MonoBehaviour {
    public GameObject mainBody;
    private Vector3 offset;
    private int lastUpMoveType;

    public EyeMove()
    {
        this.lastUpMoveType = 0;
    }

    void Start()
    {
        offset = mainBody.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate() {

        float verticalDiff = Input.GetAxis("Vertical");
        float horizontalDiff = Input.GetAxis("Horizontal");

        if (verticalDiff != 0)
        {
            if (lastUpMoveType == 3 || lastUpMoveType == 4)
                transform.RotateAround(mainBody.transform.position, new Vector3(0.0f, 1.0f, 0.0f), ((lastUpMoveType == 4) ? -1 : 1) * 90f);
            transform.position = mainBody.transform.position - new Vector3(
                offset.x,
                offset.y,
                offset.z * ((Input.GetAxis("Vertical") >= 0.0f) ? -1 : 1)
                );
            lastUpMoveType = (verticalDiff >=0) ? 0 : 1;
        }
        else if (horizontalDiff != 0)
        {
            if (lastUpMoveType == 3 || lastUpMoveType == 4)
                transform.RotateAround(mainBody.transform.position, new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * ((lastUpMoveType == 4) ? -1 : 1) * 90f);
            transform.position = mainBody.transform.position - offset;

            transform.RotateAround(mainBody.transform.position, new Vector3(0.0f, 1.0f, 0.0f), ( (horizontalDiff >= 0) ? -1 : 1 ) * 90f);
            lastUpMoveType = (horizontalDiff >= 0) ? 3 : 4;
        }
        else
        {
            ;

        }
        // vertical - sus/jos
        // horizontal - stanga/dreapta
	}
}
