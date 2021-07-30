using System;
using System.Collections;
using UnityEngine;

public class UISingleton<T> : MonoBehaviour where T : UISingleton<T>
{
    private static volatile T instance;

    [SerializeField] private bool hideOnStart;
    [SerializeField] private GameObject visual;

    public static T ins
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType(typeof(T)) as T;
            return instance;
        }
    }

    public virtual void OnClick()
    {
        Hide();
    }

    // Initialize singleton and if true disable the object
    private void Awake()
    {
        if (ins.hideOnStart) Hide();
    }

    public void Show(float delayForShow = -1f)
    {
        StartCoroutine(DelayedShow(delayForShow));
    }

    private IEnumerator DelayedShow(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        visual.SetActive(true);
    }

    public void Hide()
    {
        visual.SetActive(false);
    }
}