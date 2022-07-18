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
}
