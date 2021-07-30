using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private IntPref _levelCounter;
    public static Level activeLevel;
    public static Transform thrash;

    private void Awake()
    {
        GameData.Instance = Resources.Load<GameData>("GameData");
        GameData.Instance.Initialize();

        thrash = new GameObject().transform;
        thrash.name = "Thrash";
        _levelCounter = new IntPref("LevelCounter");

        LoadLevel();

        //Load the same level
        TapToRestart.OnTapToRestart += LoadLevel;

        //Increment current level index and load level
        TapToContinue.OnTapToContinue += IncrementLevelCounter;
        TapToContinue.OnTapToContinue += LoadLevel;
    }

    public void LoadLevel()
    {
        ClearThrash();
        var newLevel = GameData.Instance.levels.GetElement(_levelCounter);
        activeLevel = Instantiate(newLevel, thrash);
        activeLevel.LoadLevel();
        TapToStart.ins.Show();
    }

    // TODO: On level completed
    public void IncrementLevelCounter()
    {
        _levelCounter.IncrementValue();
    }

    public void ClearThrash()
    {
        var childCount = thrash.childCount;
        for (var i = 0; i < childCount; i++)
        {
            Destroy(thrash.GetChild(i).gameObject);
        }
    }
}