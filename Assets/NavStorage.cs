using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NavStorage", menuName = "Navigation Storage", order = 0)]
public class NavStorage : ScriptableObject {
    public List<Node> nodes;
    public List<NodeVolume> volumes;
}
