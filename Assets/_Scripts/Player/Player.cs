using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject visual;
    [SerializeField] private Image feverBar;
    [SerializeField] private GameObject barCanvas;

    private float _feverRatio;

    private float feverRatio
    {
        get => _feverRatio;
        set
        {
            _feverRatio = Mathf.Clamp01(value);
            feverBar.fillAmount = _feverRatio;
        }
    }

    private GameData _data;
    private float _delayDuration;
    private bool _feverIncreasedRecently;

    private void Awake()
    {
        _data = GameData.Instance;
        TapToStart.OnTapToStart += StartBehaviour;
    }

    private void OnDestroy()
    {
        TapToStart.OnTapToStart -= StartBehaviour;
    }

    private void StartBehaviour()
    {
        StartCoroutine(DoMovement());
    }

    private IEnumerator DoMovement()
    {
        GetComponentInChildren<CinemachineVirtualCamera>().transform.parent = transform.parent;

        yield return new WaitForSeconds(0.5f);

        var waypoints = GameManager.activeLevel.waypoints;
        if (!waypoints.Any()) Debug.LogError("No waypoint found");
        var currentIndex = 0;

        while (true)
        {
            var target = waypoints[currentIndex];
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < _data.playerShootDistance && target.IsCleared())
            {
                StartCoroutine(nameof(DoFeverBar));
                StartCoroutine(nameof(DoFeverDecreaseDelay));
                yield return StartCoroutine(DoShooting(target));
            }

            if (distance < Mathf.Epsilon)
            {
                currentIndex++;
                if (currentIndex >= waypoints.Count)
                {
                    TapToContinue.ins.Show(1f);
                    yield break;
                }
            }

            AdjustRotation(waypoints[currentIndex].transform.position);

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex].transform.position, _data.playerSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator DoFeverBar()
    {
        while (true)
        {
            if (!_feverIncreasedRecently)
                feverRatio -= _data.playerFeverDecreaseSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DoShooting(Platform targetPlatform)
    {
        var currentBulletCount = targetPlatform.GetBulletCount();
        while (true)
        {
            if (Input.GetMouseButton(0) && --currentBulletCount >= 0)
            {
                var newBullet = Instantiate(_data.bullet, shootingPoint.position, Quaternion.identity, GameManager.thrash);
                newBullet.Shoot(transform.forward, transform);
                feverRatio += _data.playerFeverIncreaseSpeed;
                _delayDuration += _data.playerFeverDecreaseDelayDuration;
                var cooldown = Mathf.Lerp(_data.playerMaximumShootCooldown, _data.playerMinimumShootCooldown, _feverRatio);
                yield return new WaitForSeconds(cooldown);
            }

            if (!targetPlatform.IsCleared())
            {
                StopCoroutine(nameof(DoFeverBar));
                StopCoroutine(nameof(DoFeverDecreaseDelay));
                yield return new WaitForSeconds(_data.playerOnPlatformClearedWaitDuration);
                break;
            }

            yield return null;
        }
    }
    
    private IEnumerator DoFeverDecreaseDelay()
    {
        while (true)
        {
            _delayDuration -= Time.deltaTime;
            _delayDuration = Mathf.Clamp(_delayDuration, 0, _data.playerFeverDecreaseDelayDuration);
            _feverIncreasedRecently = _delayDuration > 0f;

            yield return null;
        }
    }

    private void AdjustRotation(Vector3 targetPosition)
    {
        var delta = transform.position - targetPosition;
        delta.y = 0;
        delta = delta.normalized;
        delta *= -1;
        var rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(delta, Vector3.up), _data.playerRotationSpeed * Time.deltaTime);
        transform.rotation = rot;
    }

    public void Shatter(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DoShatter(duration));
    }

    private IEnumerator DoShatter(float duration)
    {
        yield return new WaitForSeconds(duration);
        visual.SetActive(false);
        barCanvas.SetActive(false);
        var shatteredPieces = GetComponentsInChildren<Rigidbody>();
        foreach (var shatteredPiece in shatteredPieces)
        {
            shatteredPiece.isKinematic = false;
            shatteredPiece.AddExplosionForce(0, transform.position, 5f, _data.playerShatterForce);
        }

        TapToRestart.ins.Show(1.5f);
    }
}