using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Track track;

    public Vector3 direction;
    public float distance;

    public Transform parent;
    public Transform child;
    public Transform altChild;

    public void Setup (Track t, Transform p, Transform c, Transform alt = null)
    {
        track = t;
        parent = p;
        child = c;
        altChild = alt;
    }

    public bool add_new_node;
    public bool remove_node;


    private void OnValidate()
    {
        if (add_new_node)
        {
            track.AddNode(this);
            add_new_node = false;
        }
        if (remove_node)
        {
            track.RemoveNode(this);
        }

        track.UpdateAllNodes();
    }

    public Vector3[] interpolation;

    //Final calculation of the spline segment
    public Vector3 GetPosition (float t)
    {
        return ((interpolation[0] * t + interpolation [1])*t + interpolation[2])*t + transform.position;

        return Vector3.Lerp(transform.position, child.position, t);
    }

    //Spline segment derivative
    public Vector3 GetDirection(float t)
    {
        return ((3*interpolation[0] * t + 2*interpolation[1]) * t + interpolation[2]).normalized;
    }

    public void UpdateConstants ()
    {
        //Calculate interpolation constants
        interpolation = new Vector3[3];

        Vector3 p0 = parent.position;
        p0.y = 0;
        Vector3 p1 = transform.position;
        p1.y = 0;
        Vector3 p2 = child.position;
        p2.y = 0;
        Vector3 p3 = child.GetComponent<Node>().child.position;
        p3.y = 0;

        float t1 = Mathf.Sqrt(Vector3.Distance(p1, p0));
        float t2 = t1+Mathf.Sqrt(Vector3.Distance (p2, p1));
        float t3 = t2+Mathf.Sqrt(Vector3.Distance(p3, p2));

        var m1 = (t2 - t1)*((p1 - p0) / t1 - (p2 - p0) / t2 + (p2 - p1) / (t2 - t1));
        var m2 = (t2 - t1) * ((p2 - p1) / (t2-t1) - (p3 - p1) / (t3-t1) + (p3 - p2) / (t3 - t2));

        interpolation[2] = m1;
        interpolation[1] = -3 * p1 + 3 * p2 - 2*m1 - m2;
        interpolation[0] = 2*p1 - 2*p2 + m1 + m2;


        //Calculate distance via subdevision
        distance = 0;

        for (int i = 1; i <= 32; i++)
        {
            distance += Vector3.Distance(GetPosition((i - 1) / 32f), GetPosition(i/32f));
        }
    }
}
