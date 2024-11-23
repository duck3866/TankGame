using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position { get; set; }
    public Node Parent { get; set; }
    public float GCost { get; set; }// 이동 비용(시작->현재)
    public float HCost { get; set; }// 휴리스틱 비용(현재->목표)
    public float FCost => GCost + HCost; // 총비용

    public Node(Vector3 position)
    {
        Position = position;
        Parent = null;
        GCost = 0;
        HCost = 0;
    }
}
