using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] PlatformData[] platformData;
    [HideInInspector] public List<Platform> waypoints;

    public void LoadLevel()
    {
        waypoints = new List<Platform>();

        var yMirrot = Quaternion.Euler(0, 180, 0);
        var gData = GameData.Instance;
        var currentPosition = Vector3.zero;
        var currentDirection = Quaternion.identity;

        foreach (var pData in platformData)
        {
            currentDirection *= Quaternion.Euler(0, pData.angleForBridge, 0);
            currentPosition += currentDirection * Vector3.forward * pData.lengthOfBridge;

            CreatePlatform(gData, currentPosition, pData);
            if (pData.lengthOfBridge < Mathf.Epsilon) continue;

            CreateBridge(currentDirection, yMirrot, gData, currentPosition, pData);
        }

        //Create player
        Instantiate(gData.Player, transform.position, Quaternion.identity, transform);
    }

    // Create bridge between platforms
    private void CreateBridge(Quaternion currentDirection, Quaternion yMirrot, GameData gData, Vector3 currentPosition, PlatformData pData)
    {
        var bridgeDirection = currentDirection * yMirrot;
        var newBridge = Instantiate(gData.bridgeMesh, currentPosition, bridgeDirection, transform);
        newBridge.transform.localScale = new Vector3(1, 1, pData.lengthOfBridge);
    }

    // Create platform
    private void CreatePlatform(GameData gData, Vector3 currentPosition, PlatformData pData)
    {
        var newPlatform = Instantiate(gData.platformMesh, currentPosition, Quaternion.identity, transform);
        newPlatform.transform.localScale = new Vector3(pData.platformScale, 0.1f, pData.platformScale);
        newPlatform.Initialize(pData);
        waypoints.Add(newPlatform);
    }

    // Visualize level
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            var startPosition = Vector3.zero;
            foreach (var waypoint in waypoints)
            {
                Gizmos.color = Color.white;
                var pos = waypoint.transform.position;
                Gizmos.DrawLine(startPosition + Vector3.up, pos + Vector3.up);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pos, 0.25f);
                startPosition = pos;
            }

            return;
        }

        var gData = Resources.Load<GameData>("GameData");
        var currentPosition = Vector3.zero;
        var currentDirection = Quaternion.identity;

        foreach (var pData in platformData)
        {
            Gizmos.color = Color.blue;
            currentDirection *= Quaternion.Euler(0, pData.angleForBridge, 0);
            var previousPosition = currentPosition;
            currentPosition += currentDirection * Vector3.forward * pData.lengthOfBridge;
            Gizmos.DrawMesh(gData.bridgeMesh.GetComponentInChildren<MeshFilter>().sharedMesh, 0, (currentPosition + previousPosition) / 2f, currentDirection, new Vector3(1, 0.1f, pData.lengthOfBridge));
            Gizmos.DrawMesh(gData.platformMesh.GetComponent<MeshFilter>().sharedMesh, 0, currentPosition, Quaternion.identity, new Vector3(pData.platformScale, 0.1f, pData.platformScale));
            foreach (var obstacleData in pData.obstacleData)
            {
                Gizmos.color = Color.black;
                obstacleData.ShowGizmos(currentPosition);
            }
        }
    }
}