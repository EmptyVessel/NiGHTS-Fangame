using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Simple first person camera for debugging
public class Dev_Camera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float speed = 10;
    private float rotation_speed = 10;

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        var spd = speed * Time.deltaTime;

        if (keyboard.dKey.isPressed)
        {
            this.transform.Translate(Vector3.right * spd);
        }
        if (keyboard.aKey.isPressed)
        {
            this.transform.Translate(-Vector3.right * spd);
        }
        if (keyboard.wKey.isPressed)
        {
            this.transform.Translate(Vector3.forward * spd);
        }
        if (keyboard.sKey.isPressed)
        {
            this.transform.Translate(-Vector3.forward * spd);
        }
        if (keyboard.spaceKey.isPressed)
        {
            this.transform.Translate(Vector3.up * spd);
        }
        if (keyboard.leftShiftKey.isPressed)
        {
            this.transform.Translate(-Vector3.up* spd);
        }

        transform.Rotate(Vector2.up, mouse.delta.x.ReadValue()*Time.deltaTime*rotation_speed, Space.World);
        transform.Rotate(Vector2.right, -mouse.delta.y.ReadValue() * Time.deltaTime * rotation_speed);
    }
}
