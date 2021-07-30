using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private PlatformData _data;
    private Queue<Breakable> _breakables;
    private List<Obstacle> _obstacles;
    private float heightOffset;
    private Transform _obstacleParent;

    public void Initialize(PlatformData pData)
    {
        _obstacleParent = new GameObject().transform;
        _obstacleParent.parent = transform;
        _obstacleParent.localPosition = Vector3.zero;

        _data = pData;

        _breakables = new Queue<Breakable>();
        _obstacles = new List<Obstacle>();

        GenerateBreakables(pData);
        GenerateObstacles();
    }

    private void GenerateBreakables(PlatformData pData)
    {
        var colorCycler = new CyclingList<Color>(pData.breakableColorPalette);
        for (var i = 0; i < _data.breakableCount; i++)
        {
            var newBreakable = Instantiate(_data.breakable, spawnPoint.position + Vector3.up * heightOffset, Quaternion.identity);
            newBreakable.Initialize(colorCycler.GetElement(i));
            newBreakable.transform.parent = spawnPoint.transform;

            heightOffset += _data.breakable.height;
            _breakables.Enqueue(newBreakable);
        }
    }


    private void GenerateObstacles()
    {
        foreach (var obstacleData in _data.obstacleData)
        {
            var rotation = Quaternion.Euler(0, obstacleData.angleFromForward, 0);
            var pos = (rotation * Vector3.forward) * obstacleData.distanceFromCenter;

            var newObstacle = Instantiate(obstacleData.obstacle);
            newObstacle.transform.parent = _obstacleParent;
            newObstacle.transform.localRotation = rotation;
            newObstacle.transform.localPosition = pos;
            _obstacles.Add(newObstacle);
        }
    }

    public void RemoveBreakable()
    {
        if (!_breakables.Any()) return;
        var breakableToRemove = _breakables.Dequeue();
        var distanceToMoveDown = breakableToRemove.height;
        foreach (var breakable in _breakables)
        {
            breakable.MoveDown(distanceToMoveDown);
        }

        if (_breakables.Any()) return;
        enabled = false;
        foreach (var obstacle in _obstacles)
        {
            obstacle.Dissolve();
        }
    }

    private void Update()
    {
        spawnPoint.Rotate(0, _data.platformRotationSpeed * Time.deltaTime, 0);
        _obstacleParent.Rotate(0, _data.obstacleRotationSpeed * Time.deltaTime, 0);
    }

    public bool IsCleared()
    {
        return _breakables.Any();
    }

    // Get maximum bullet count for this platform
    public int GetBulletCount()
    {
        return _breakables.Count;
    }
}