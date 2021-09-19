using UnityEngine;

public abstract class MothState : MonoBehaviour
{
    public delegate void MothStateEvent();
    public static event MothStateEvent OnHitByWave;
    public static event MothStateEvent OnBecomeActive;
    public static event MothStateEvent OnSitOnBeard;
    public static event MothStateEvent OnFlyOffBeard;

    private const float FLY_EPSILON = 0.01f;
    private const int DEBUG_DRAW_COUNT = 100;

    public GameObject Sprite;

    protected virtual void OnEnable()
    {
        GameManager.OnLose += OnGameLose;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnLose -= OnGameLose;
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        DrawDebugPath();
#endif
    }

    private void DrawDebugPath()
    {
        for (var i = 0; i < DEBUG_DRAW_COUNT; ++i)
        {
            var t0 = i / (float) DEBUG_DRAW_COUNT;
            var t1 = (i + 1) / (float) DEBUG_DRAW_COUNT;
            Debug.DrawLine(GetPositionByTimer(t0), GetPositionByTimer(t1));
        }
    }

    protected abstract Vector2 GetPositionByTimer(float timer);

    protected float GetAngleByTimer(float t)
    {
        var t0 = t - FLY_EPSILON;
        if (t0 < 0.0f)
        {
            t0 = 1.0f - t0;
        }
        var t1 = t + FLY_EPSILON;
        if (t1 > 1.0f)
        {
            t1 = t1 - 1.0f;
        }

        var delta = GetPositionByTimer(t0) - GetPositionByTimer(t1);
        return Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 90.0f;
    }

    protected void EmitHitByWave()
    {
        if (OnHitByWave != null)
        {
            OnHitByWave();
        }
    }

    protected Vector2 GetOnBeardPosition()
    {
        // TODO: magic numbers
        return Random.insideUnitCircle * 1.1f + new Vector2(0.0f, -3.5f);
    }

    protected void EmitOnBecomeActive()
    {
        if (OnBecomeActive != null)
        {
            OnBecomeActive();
        }
    }

    private void OnGameLose()
    {
        // TODO: shitty code
        Sprite.SetActive(false);
        GetComponent<ParticleSystem>().Play();
    }

    protected void EmitSitOnBeard()
    {
        if (OnSitOnBeard != null)
        {
            OnSitOnBeard();
        }
    }

    protected void EmitFlyOffBeard()
    {
        if (OnFlyOffBeard != null)
        {
            OnFlyOffBeard();
        }
    }
}