using System;

public class TapToStart : UISingleton<TapToStart>
{
    public static event Action OnTapToStart;

    public override void OnClick()
    {
        base.OnClick();
        OnTapToStart?.Invoke();
    }
}