using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NavStorage", menuName = "Navigation Storage", order = 0)]
public class NavStorage : ScriptableObject {
    public List<Node> nodes;
    public List<NodeVolume> volumes;

    public int nodeCount, volumeCount;
    public List<Node> Nodes
    {
        get
        {
            return nodes;
        }

        set
        {
            nodes = value;
            nodeCount = nodes.Count;
        }
    }

    public List<NodeVolume> Volumes
    {
        get
        {
            return volumes;
        }

        set
        {
            volumes = value;
            volumeCount = volumes.Count;
        }
    }
}
