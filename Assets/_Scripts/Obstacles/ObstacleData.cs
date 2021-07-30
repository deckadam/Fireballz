using System;
using UnityEngine;

[Serializable]
public class ObstacleData
{
    public Obstacle obstacle;
    public float angleFromForward;
    public float distanceFromCenter;

    public void ShowGizmos(Vector3 currentPosition)
    {
        var mesh = obstacle.GetComponentInChildren<MeshFilter>().sharedMesh;
        var rotation = Quaternion.Euler(0, angleFromForward, 0);
        var scale = obstacle.transform.GetChild(0).localScale;
        var pos = currentPosition + (rotation * Vector3.forward) * distanceFromCenter;
        Gizmos.DrawMesh(mesh, 0, pos, rotation, scale);
    }
}