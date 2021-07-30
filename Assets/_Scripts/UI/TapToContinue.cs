using System;

public class TapToContinue : UISingleton<TapToContinue>
{
    public static event Action OnTapToContinue;

    public override void OnClick()
    {
        base.OnClick();
        OnTapToContinue?.Invoke();
    }
}