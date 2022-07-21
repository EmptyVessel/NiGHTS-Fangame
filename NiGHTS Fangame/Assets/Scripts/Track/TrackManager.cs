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

    public Vector3 GetDirection(float t)
    {
        return currentNode.GetDirection (t);
    }

    public Vector3 GetDirection(Vector3 pos)
    {
        return currentNode.GetDirection(GetDistanceAlongEdge (pos));
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
        pos.y = currentNode.transform.position.y;
        var v1 = pos - currentNode.transform.position;
        float t = Vector3.Dot(v1, currentNode.direction) / currentNode.distance;
        float nt = t;
        float dT = 0.1f;

        float dis = (GetPosition (t)-pos).sqrMagnitude;
        float d;

        for (int i = 0; i < 8; i++)
        {
            d = (GetPosition(t + dT) - pos).sqrMagnitude;

            if ((GetPosition(t+dT) - pos).sqrMagnitude < dis)
            {
                dis = d;
                nt = t+dT;
            }

            d = (GetPosition(t + dT) - pos).sqrMagnitude;

            if ((GetPosition(t - dT) - pos).sqrMagnitude < dis)
            {
                dis = d;
                nt = t-dT;
            }

            t = nt;
            dT /= 2f;
        }

        print(Vector3.Dot ((GetPosition(t) - pos).normalized, GetDirection(t)));

        return t;
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
