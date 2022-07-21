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
    public Camera camera;

    void FixedUpdate()
    {
        Move();
        trackManager.UpdateNode(transform.position);
    }

    bool face = true;

    private void Move()
    {
        var keyboard = Keyboard.current;

        if (keyboard.dKey.isPressed)
        {
            this.transform.Translate(transform.GetChild(0).forward * speed * Time.deltaTime);
            face = true;
        }
        if (keyboard.aKey.isPressed)
        {
            this.transform.Translate(transform.GetChild(0).forward * speed * Time.deltaTime);
            face = false;
        }
        if (keyboard.wKey.isPressed)
        {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (keyboard.sKey.isPressed)
        {
            this.transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }

        var q = new Quaternion();
        q.SetLookRotation(trackManager.GetDirection(transform.position, face));

        transform.GetChild (0).rotation = q;

        if (face)
        {
            camera.transform.position = transform.position + transform.GetChild(0).right*20;
        } else
        {
            camera.transform.position = transform.position - transform.GetChild(0).right * 20;
        }

        camera.transform.LookAt(transform.position);
    }
}
