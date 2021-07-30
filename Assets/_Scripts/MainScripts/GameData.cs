using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Game Data", order = 0)]
public class GameData : ScriptableObject
{
    public static GameData Instance;

    [SerializeField] private List<Level> levelPrefabs;
    public CyclingList<Level> levels;
    public Platform platformMesh;
    public Player Player;
    public GameObject bridgeMesh;

    [Space]
    public float playerSpeed;

    public float playerRotationSpeed;
    public float playerShootDistance;
    public float playerMaximumShootCooldown;
    public float playerMinimumShootCooldown;
    public float playerOnPlatformClearedWaitDuration;
    public float playerShatterForce;
    public float playerFeverIncreaseSpeed;
    public float playerFeverDecreaseSpeed;
    public float playerFeverDecreaseDelayDuration;

    [Space]
    public Bullet bullet;

    public float bulletSpeed;
    public float bulletJumpBackPower;
    public float bulletJumpBackDuration;

    [Space]
    public float breakableDestructionDuration;

    public float breakableMoveDownSpeed;

    public void Initialize()
    {
        levels = new CyclingList<Level>(levelPrefabs);
    }
}