using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Track : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> nodes = new List<GameObject>();

    public bool add_new_node;
    public bool update_all_nodes;

    private void OnValidate()
    {
        //adds a new node at the end of the track
        if (add_new_node)
        {
            AddNode(nodes[0].GetComponent <Node>().parent.GetComponent <Node>());
            add_new_node = false;
        }
        //Manual node update
        if (update_all_nodes)
        {
            UpdateAllNodes();
            update_all_nodes = false;
        }
    }

    //subdivides a node and its child
    public void AddNode (Node n)
    {
        var g = new GameObject ("node_" + nodes.Count);
        g.transform.parent = this.transform;
        var nd = g.AddComponent<Node>();

        g.transform.position = (n.transform.position + n.child.position)/2f;

        nd.Setup(this, n.transform, n.child);
        n.child.GetComponent<Node>().parent = g.transform;
        n.child = g.transform;

        nodes.Add(g);
        UpdateAllNodes();
    }

    //Removes node
    public void RemoveNode (Node n)
    {
        if (nodes.Count == 2)
        {
            Debug.LogException(new System.Exception("can only remove up to 2 nodes"));
        }
        if (n == nodes [0].GetComponent <Node>())
        {
            Debug.LogException(new System.Exception ("can't remove starting node"));
            return;
        }



        if (n.child.GetComponent <Node>().parent == n.transform)
        {
            n.child.GetComponent<Node>().parent = n.parent;
        }
        if (n.parent.GetComponent<Node>().child == n.transform)
        {
            n.parent.GetComponent<Node>().child = n.child;
        }

        nodes.Remove(n.gameObject);


        UnityEditor.EditorApplication.delayCall += () =>
        {
            DestroyImmediate(n.gameObject);
        };

        UpdateAllNodes();
    }

    //Renames the nodes in order of their position along the graph
    public void UpdateAllNodes ()
    {
        Node currNode = nodes[0].GetComponent <Node>();
        Vector3 v;
        int i = 0;

        while (!(i!=0 && currNode == nodes[0].GetComponent<Node>()))
        {
            currNode.gameObject.name = "node_" + i;
            v = currNode.child.position-currNode.transform.position;
            currNode.distance = v.magnitude;
            currNode.direction = v.normalized;

            currNode = currNode.child.GetComponent<Node>();

            i++;
        }
    }

    public bool draw_Gizmos;
    public GUIStyle style;

    private void OnDrawGizmos()
    {
        if (draw_Gizmos)
        {
            //Draw backwards directed nodes
            for (int i = 0; i < nodes.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(nodes[i].transform.position, nodes[i].GetComponent<Node>().parent.position);
            }

            //Draw forward directed nodes
            for (int i = 0; i < nodes.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(nodes [i].transform.position, nodes[i].GetComponent<Node>().child.position);

                if (nodes[i].GetComponent<Node>().altChild != null)
                {
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i].GetComponent<Node>().altChild.position);
                }

                Handles.Label(nodes[i].transform.position, nodes[i].name, style);
            }
        }  
    }
}
