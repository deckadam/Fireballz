using UnityEngine;

public class IntPref
{
    private string _name;
    private int _value = -1;

    public IntPref(string name)
    {
        _name = name;
    }

    public int GetValue()
    {
        if (_value == -1)
        {
            _value = PlayerPrefs.GetInt(_name);
        }

        return _value;
    }

    public void SetValue(int newValue)
    {
        _value = newValue;
        PlayerPrefs.SetInt(_name, _value);
    }

    public void IncrementValue(int value = 1)
    {
        SetValue(GetValue() + value);
    }

    public static implicit  operator int(IntPref b) => b.GetValue();
}