using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetTrack(0);
    }

    public int currentTrack;

    public List<Track> tracks;
    public Node currentNode;

    public float GetDistanceAlongEdge(Vector3 pos)
    {
        //Vector from the node
        var v1 = pos-currentNode.transform.position;

        return Vector3.Dot(v1, currentNode.direction);
    }

    public void SetTrack (int i)
    {
        currentTrack = i;
        currentNode = tracks[i].nodes[0].GetComponent <Node>();
    }

    public Vector3 GetDirection (Vector3 pos)
    {
        float d = GetDistanceAlongEdge(pos);
        float t = d / currentNode.distance;

        return currentNode.direction;
    }

    public void UpdateNode (Vector3 pos)
    {
        float dis = GetDistanceAlongEdge(pos);

        if (dis > currentNode.distance)
        {
            currentNode = currentNode.child.GetComponent <Node>();
        }
        else if (dis < 0)
        {
            currentNode = currentNode.parent.GetComponent<Node>();
        }
    }
}
