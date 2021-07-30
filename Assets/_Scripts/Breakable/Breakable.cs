using DG.Tweening;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float height;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public void Initialize(Color color)
    {
        GetComponentInChildren<MeshRenderer>().material.SetColor(BaseColor, color);
    }

    public void Dissolve()
    {
        GetComponentInParent<Platform>().RemoveBreakable();
        GetComponentInChildren<Collider>().enabled = false;
        GetComponentInChildren<Renderer>().material.DOColor(Color.clear, GameData.Instance.breakableDestructionDuration);
        var currentScale = transform.localScale;
        transform.DOScale(currentScale * 1.4f, GameData.Instance.breakableDestructionDuration).OnComplete(() => Destroy(gameObject));
    }

    public void MoveDown(float distance)
    {
        transform.DOMoveY(-distance, GameData.Instance.breakableMoveDownSpeed).SetRelative(true).SetSpeedBased(true).SetEase(Ease.Linear);
    }
}