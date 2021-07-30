using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void Dissolve()
    {
        GetComponentInChildren<MeshRenderer>().material.DOColor(Color.clear, GameData.Instance.breakableDestructionDuration);
        var currentScale = transform.GetChild(0).localScale;
        transform.DOScale(currentScale * 1.4f, GameData.Instance.breakableDestructionDuration).OnComplete(() => Destroy(gameObject));
    }
}