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

    //Gets position along the track and doesn't update current node
    public Vector3 GetPosition(float dis)
    {
        var node = currentNode;

        while (dis >= node.distance)
        {
            dis -= node.distance;
            node = node.child.GetComponent<Node>();
        }
        while (dis < 0)
        {
            node = node.parent.GetComponent<Node>();
            dis += node.distance;
        }

        return node.GetPosition(dis / node.distance);
    }

    //Gets position along the track and updated the current node
    public Vector3 UpdatePosition(ref float dis)
    {
        while (dis >= currentNode.distance)
        {
            dis -= currentNode.distance;
            currentNode = currentNode.child.GetComponent<Node>();
        }
        while (dis < 0)
        {
            currentNode = currentNode.parent.GetComponent<Node>();
            dis += currentNode.distance;
        }

        return currentNode.GetPosition(dis / currentNode.distance);
    }

    //facing: true - right; false - left
    public Vector3 GetDirection(float dis, bool facing)
    {
        var v = currentNode.GetDirection (dis/currentNode.distance);

        if (facing)
        {
            return v;
        } else
        {
            return -v;
        }
    }


    //depreceated
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

    //depreceated
    public float GetDistanceAlongEdge(Vector3 pos)
    {
        //Vector from the node
        var v1 = pos - currentNode.transform.position;

        return Vector3.Dot(v1, currentNode.direction);
    }


    //Immediatly track related functions
    public void SetTrack(int i)
    {
        currentTrack = i;
        currentNode = tracks[i].nodes[0].GetComponent<Node>();
    }

    public Track GetTrack()
    {
        return tracks[currentTrack];
    }
}
