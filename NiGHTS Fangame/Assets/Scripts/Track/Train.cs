using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Train : MonoBehaviour
{
    public TrackManager trackManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = trackManager.tracks[0].nodes[0].transform.position;
    }

    public float speed;
    //public Camera camera;

    void Update()
    {
        Move();
        CorrectPosition();
        trackManager.UpdateNode(transform.position);
    }

    private void Move()
    {
        var keyboard = Keyboard.current;

        if (keyboard.dKey.isPressed)
        {
            transform.Translate(trackManager.GetDirection(transform.position) * speed * Time.deltaTime);
        }
        if (keyboard.aKey.isPressed)
        {
            transform.Translate(-trackManager.GetDirection(transform.position) * speed * Time.deltaTime);
        }
        if (keyboard.wKey.isPressed)
        {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (keyboard.sKey.isPressed)
        {
            this.transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }

        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.LookRotation (trackManager.GetDirection(transform.position)), Time.deltaTime*4);

        Camera();
    }

    void Camera ()
    {
        GetComponent<Camera>().transform.position = transform.position + transform.GetChild(0).right * 20;
        GetComponent<Camera>().transform.LookAt(transform.position);
    }

    void CorrectPosition ()
    {
        
    }
}
