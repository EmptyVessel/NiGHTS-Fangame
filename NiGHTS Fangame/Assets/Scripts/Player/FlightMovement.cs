using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FlightMovement : MonoBehaviour
{
    [Header("Prerequisites")]
    public TrackManager trackManager;
    public CharacterController controller;
    public PlayerInputActions pia;

    [Header("Movement Calculation")]
    public Vector2 controlVector = Vector2.zero;    // Controller vector
    public Vector3 tempVector = Vector2.zero;   // First movement vector
    public Vector3 moveVector = Vector3.zero;   // Master movement vector
    public float correctedY = 0;
    public Vector2 lastDir;     // Dash vector
    public float speed = 20f;
    public bool drillDash;
    public float xLag = 0f;

    [Header("Dev Options")]
    public bool noFall = false;
    public bool followTrack = true;

    void Start()
    {
        // Get controller and move to node 0
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = trackManager.tracks[0].nodes[0].transform.position;
        controller.enabled = true;

        // Enables Player Input
        pia = new PlayerInputActions();
        pia.Player.Enable();
    }

    void FixedUpdate()
    {
        drillDash = ConvertToBool(pia.Player.AButton.ReadValue<float>());
        MoveCalc();
        RailLock();
        controller.Move(moveVector);

        //Track snapping, make sure to rewrite it in a way that's good for you
        var v = trackManager.GetPosition(transform.position);
        controller.Move(new Vector3 (v.x, transform.position.y, v.z)-transform.position);
    }

    private void MoveCalc()
    {
        trackManager.UpdateNode(transform.position);

        // Add lag to X axis and resolve the control vector
        float xOld = controlVector.x;
        controlVector = pia.Player.LeftStick.ReadValue<Vector2>();
        xLag = Mathf.Lerp(xLag, xOld, Time.deltaTime * (2.5f - pia.Player.AButton.ReadValue<float>()));

        if (!drillDash)
        {
            // Slow Descend when not moving. Turn on "noFall" to disable this
            if (controlVector.x == 0 && controlVector.y == 0 && (moveVector.x < 0.1 && moveVector.x > -0.1))
            {
                if (!noFall) tempVector = new Vector3(0, -0.075f, 0);
                if (noFall) tempVector = Vector3.zero;
            }
            // Standard movement
            else tempVector = (transform.forward * xLag + transform.up * controlVector.y) * (Time.deltaTime * speed);

            // Add lag to Y input and set master movement vector
            correctedY = Mathf.Lerp(correctedY, tempVector.y, Time.deltaTime * 6);
            moveVector = new Vector3(tempVector.x, correctedY, tempVector.z);
        }
        if (drillDash)
        {
            // Dash in last known direction when not moving
            if (controlVector.x == 0 && controlVector.y == 0)
            {
                tempVector = (transform.forward * lastDir.x + transform.up * lastDir.y) * (Time.deltaTime * (speed * 1.75f));
            }
            // Standard dashing movement
            else
            {
                lastDir = controlVector;
                tempVector = (transform.forward * xLag + transform.up * controlVector.y) * (Time.deltaTime * (speed * 1.75f));
            }
            // Add lag to Y input and set master movement vector
            correctedY = Mathf.Lerp(correctedY, tempVector.y, Time.deltaTime * 1.5f);
            moveVector = new Vector3(tempVector.x, correctedY, tempVector.z);
        }
    }

    private void RailLock()
    {
        // Rotate on track. Disable "followTrack" to freely move
        if (followTrack)
        {
            if (controlVector.x >= 0)
            {
                Vector3 lookVector = new Vector3(trackManager.currentNode.child.transform.position.x, transform.position.y, trackManager.currentNode.child.transform.position.z) - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookVector), Time.deltaTime * (1.5f + pia.Player.AButton.ReadValue<float>()));
            }
            if (controlVector.x < 0)
            {
                Vector3 lookVector = new Vector3(trackManager.currentNode.transform.position.x, transform.position.y, trackManager.currentNode.transform.position.z) - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookVector) * Quaternion.Euler(0, 180, 0), Time.deltaTime * (1.5f + pia.Player.AButton.ReadValue<float>()));
            }
        }
        // Rotate with bumpers or Q/E (Not going to be a constant feature, just here in case it's needed)
        if (!followTrack)
        {
            if (pia.Player.LeftBumper.ReadValue<float>() == 1)
            {
                if (drillDash) transform.Rotate(new Vector3(0, -2, 0));
                if (!drillDash) transform.Rotate(new Vector3(0, -1, 0));
            }
            if (pia.Player.RightBumper.ReadValue<float>() == 1)
            {
                if (drillDash) transform.Rotate(new Vector3(0, 2, 0));
                if (!drillDash) transform.Rotate(new Vector3(0, 1, 0));
            }
        }
    }

    // Converts 0/1 value to boolean
    // If we end up needing something like this a lot, I can make it static and put it in its own script so it can be referenced whenever. Buttons read as a float in new input system, so it might just be needed often.
    bool ConvertToBool(float num)
    {
        return Mathf.Approximately(Mathf.Min(num, 1), 1);
    }
}