using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : MonoBehaviour
{

    public float speed;
    private bool horizontalDirection;
    private bool verticalDirection;
    public MazeGenerator field;

    //private Rigidbody rb;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(
            horizontal,
            0f,
            vertical);
    }

    public void OnCollisionEnter(Collision collision)
    {

    }

    public bool OrinetationVertical()
    {
        return (Input.GetAxis("Vertical") >= 0.0f) ? true : false;
    }

    public bool OrinetationHorizontal()
    {
        return (Input.GetAxis("Horizontal") >= 0.0f) ? true : false;
    }
}
