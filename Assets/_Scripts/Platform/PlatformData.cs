using System;
using UnityEngine;

[Serializable]
public class PlatformData
{
    public float platformRotationSpeed;
    public float platformScale;
    public float lengthOfBridge;
    public float angleForBridge;
    public int breakableCount;
    public Breakable breakable;
    public ObstacleData[] obstacleData;
    public float obstacleRotationSpeed;

    public Color[] breakableColorPalette;
}