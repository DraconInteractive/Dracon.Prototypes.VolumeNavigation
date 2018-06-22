using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour {
    public List<Node> allNodes = new List<Node>();
    public List<NodeVolume> allVolumes = new List<NodeVolume>();
    public int gridSize;
    public float edgeRange;
    public int nodeSpread;
    public LayerMask obstructionMask;

    public NavStorage storage;
	// Use this for initialization
	void Start () {
        Do();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [ContextMenu("Do")]
    public void Do()
    {
        StartCoroutine(DoGeneration());
    }

    [ContextMenu("Wipe")]
    public void Wipe ()
    {
        allNodes.Clear();
        allVolumes.Clear();
    }

    [ContextMenu("Save")]
    public void SaveNav ()
    {
        if (storage != null)
        {
            storage.Nodes = new List<Node>(allNodes);
            storage.Volumes = new List<NodeVolume>(allVolumes);
        }
    }
    [ExecuteInEditMode]
    IEnumerator DoGeneration ()
    {
        if (storage != null)
        {
            allNodes = new List<Node>(storage.Nodes);
            allVolumes = new List<NodeVolume>(storage.Volumes);
        }
        else
        {
            yield return StartCoroutine(GenerateNodes());
            yield return StartCoroutine(GenerateEdges());
            yield return StartCoroutine(GenerateVolumes());
            yield return StartCoroutine(GetObstacles());
        }
        
        yield break;
    }

    IEnumerator GenerateNodes ()
    {
        int counter = 0;
        for (int x = 0; x < gridSize; x += nodeSpread)
        {
            for (int y = 0; y < gridSize; y += nodeSpread)
            {
                for (int z = 0; z < gridSize; z += nodeSpread)
                {
                    Node newNode = new Node();
                    newNode.position = new Vector3(x, y, z);
                    allNodes.Add(newNode);
                }
                counter++;
                if (counter > 10)
                {
                    counter = 0;
                    yield return null;                    
                }
                
            }
        }
        print("Finished Node Generation");
        yield break;
    }

    IEnumerator GenerateEdges ()
    {
        int counter = 0;
        foreach (Node node in allNodes)
        {
            foreach (Node n in allNodes)
            {
                if (node != n && Vector3.Distance(node.position, n.position) < edgeRange)
                {
                    Edge newEdge = new Edge();
                    newEdge.endNode = n;
                    node.edges.Add(newEdge);
                }                
            }
            counter++;
            if (counter > 100)
            {
                counter = 0;
                yield return null;
            }
        }
        print("Finished Edge Generation");
        yield break;
    }

    IEnumerator GenerateVolumes ()
    {
        for (float x = nodeSpread / 2; x < gridSize - 1; x += nodeSpread)
        {
            for (float y = nodeSpread / 2; y < gridSize - 1; y += nodeSpread)
            {
                for (float z = nodeSpread / 2; z < gridSize - 1; z += nodeSpread)
                {
                    NodeVolume volume = new NodeVolume();
                    volume.centre = new Vector3(x, y, z);
                    //List<Node> neighbours = GetNodeNeighbours(volume.centre);
                    /*
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        volume.corners[i] = neighbours[i];
                    }*/
                    allVolumes.Add(volume);
                }
            }
            yield return null;
        }
        print("Finished Volume Generation");
        yield break;
    }

    IEnumerator GetObstacles ()
    {
        foreach (NodeVolume volume in allVolumes)
        {
            bool obstructed = Physics.CheckBox(volume.centre, Vector3.one * (nodeSpread / 2), Quaternion.identity, obstructionMask);
            if (obstructed)
            {
                volume.obstructed = true;
                /*
                foreach (Node n in volume.corners)
                {
                    n.obstructed = true;
                }
                */
            }
        }
        print("Finished Obstacle Detection");
        yield break;
    }

    public Node GetNodeFromPosition (Vector3 position)
    {
        Node target = null;
        float bigDist = Mathf.Infinity;
        foreach (Node node in allNodes)
        {
            float d = Vector3.Distance(node.position, position);
            if (d < bigDist)
            {
                bigDist = d;
                target = node;
            }
        }

        return target;
    }

    public List<Node> GetNodeNeighbours (Vector3 node)
    {
        List<Node> neighbours = new List<Node>();
        foreach (Node n in allNodes)
        {
            if (Vector3.Distance(node, n.position) < edgeRange)
            {
                neighbours.Add(n);
            }
        }
        return neighbours;
    }
    
    void OnDrawGizmos ()
    {
        /*
        if (allNodes.Count > 0)
        {
            
            foreach (Node n in allNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(n.position, 0.1f);
                
                foreach (Edge edge in n.edges)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(n.position, edge.endNode.position);
                }
                
            }
            
            
        }
        */
        if (allVolumes == null)
        {
            return;
        }
        foreach (NodeVolume volume in allVolumes)
        {
            if (volume.obstructed)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(volume.centre, Vector3.one * nodeSpread);
            }
            else
            {
                Gizmos.color = new Color(0, 0, 1, 0.1f);
            }

        }
    }

}

public class Node {
    public Vector3 position;
    public List<Edge> edges = new List<Edge>();
    public bool obstructed = false;
}

public class Edge
{
    public Node endNode;
}

[System.Serializable]
public class NodeVolume
{
    public Node[] corners = new Node[8];
    public Vector3 centre;
    public bool obstructed = false;
}
