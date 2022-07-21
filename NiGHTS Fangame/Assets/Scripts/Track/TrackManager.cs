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
    //Paremetric relation
    public Vector3 GetPosition(float t)
    {
        return currentNode.GetPosition(t);
    }

    //Position relation
    public Vector3 GetPosition(Vector3 pos)
    {
        return currentNode.GetPosition(GetDistanceAlongEdge (pos));
    }

    //facing: true - right; false - left
    public Vector3 GetDirection(float t, bool facing)
    {
        var v = currentNode.GetDirection (t);

        if (facing)
        {
            return v;
        } else
        {
            return -v;
        }
    }

    public Vector3 GetDirection(Vector3 pos, bool facing)
    {
        print(GetDistanceAlongEdge(pos));
        var v = currentNode.GetDirection(GetDistanceAlongEdge (pos));

        if (facing)
        {
            return v;
        }
        else
        {
            return -v;
        }
    }

    public void UpdateNode (Vector3 pos)
    {
        float dis = GetDistanceAlongEdge(pos);

        if (dis > 1)
        {
            currentNode = currentNode.child.GetComponent <Node>();
        }
        else if (dis < 0)
        {
            currentNode = currentNode.parent.GetComponent<Node>();
        }
    }


    public float GetDistanceAlongEdge(Vector3 pos)
    {
        //Vector from the node
        var v1 = pos - currentNode.transform.position;

        return Vector3.Dot(v1, currentNode.direction)/currentNode.distance;
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
