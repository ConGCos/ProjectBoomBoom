using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{


    public GameObject objectToFollow;

    public float speed = 2.0f;
    public float modY = 1.0f;
    public float modX = 1.0f;
    void FixedUpdate()
    {
        float interpolation = speed * Time.deltaTime;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = this.transform.position;


        position.y = Mathf.Lerp(this.transform.position.y, (objectToFollow.transform.position.y + (mousePos.y/modY))/2, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, (objectToFollow.transform.position.x + (mousePos.x/modX))/2, interpolation);

        this.transform.position = position;
    }
}
