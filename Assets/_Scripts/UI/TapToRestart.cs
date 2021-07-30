using System;

public class TapToRestart : UISingleton<TapToRestart>
{
    public static event Action OnTapToRestart;

    public override void OnClick()
    {
        base.OnClick();
        OnTapToRestart?.Invoke();
    }
}