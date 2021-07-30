using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _shooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            _rb.DOKill();
            other.GetComponentInParent<Breakable>().Dissolve();

            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            var data = GameData.Instance;

            _rb.DOKill();
            _rb.DOJump(_shooter.position, data.bulletJumpBackPower, 1, data.bulletJumpBackDuration).SetEase(Ease.Linear);

            _shooter.GetComponent<Player>().Shatter(data.bulletJumpBackDuration);
        }
    }

    public void Shoot(Vector3 direction, Transform shooter)
    {
        _shooter = shooter;

        _rb = GetComponent<Rigidbody>();
        _rb.DOMove(direction * 100f, GameData.Instance.bulletSpeed).SetRelative(true).SetSpeedBased(true).OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        _rb.DOKill();
    }
}