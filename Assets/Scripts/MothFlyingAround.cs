using System.Collections;
using UnityEngine;


public class MothFlyingAround : MothState
{
    public MothFlyingPathCFG PathCfg;

    public float FlyAreaScale;

    public float SpeedMin;
    public float SpeedMax;
    public float Speed;

    public float ChangeSpeedSlowSecondsMin;
    public float ChangeSpeedSlowSecondsMax;
    public float ChangeSpeedFastSecondsMin;
    public float ChangeSpeedFastSecondsMax;

    private Rigidbody2D rb2D;
    private ParticleSystem particles;
    private float flyingTimer;
    private MothFlyTo flyToState;
    private MothStayStill stayStillState;

    private Vector2 initialPosition;
    private Vector2 firstStepDelta;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        flyToState = GetComponent<MothFlyTo>();
        stayStillState = GetComponent<MothStayStill>();
    }

    private void Start()
    {
        flyingTimer = 0.0f;
        Speed = SpeedMin;
        initialPosition = rb2D.position;

        PathCfg.ResetWithRandomValues();
        StartCoroutine(ChangeSpeed());
        StartCoroutine(ChangePathDirs());

        RecalcFirstStepDelta();
    }

    protected override void Update()
    {
        base.Update();

        flyingTimer += Time.deltaTime * Speed;
        if (flyingTimer > 1.0f)
        {
            flyingTimer = flyingTimer - 1.0f;
        }
        PathCfg.SlowlyChange();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.tag == "Catcher")
        {
            particles.Play();
            AudioManager.PlayHitMoth();
            enabled = false;
            EmitHitByWave();
            stayStillState.PreviousAroundPosition = flyToState.EndPosition;
            flyToState.ResetToStart(rb2D.position, GetOnBeardPosition(), stayStillState);
            flyToState.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        RecalcFirstStepDelta();

        rb2D.MovePosition(GetPositionByTimer(flyingTimer));
        rb2D.MoveRotation(GetAngleByTimer(flyingTimer));
    }

    public void ResetAfterStayingStill()
    {
        Speed = SpeedMin;
        flyingTimer = 0.0f;
    }

    private IEnumerator ChangeSpeed()
    {
        while (true)
        {
            Speed = Random.Range(SpeedMin, SpeedMin + (SpeedMax - SpeedMin) * 0.25f);
            yield return new WaitForSeconds(Random.Range(ChangeSpeedSlowSecondsMin, ChangeSpeedSlowSecondsMax));

            Speed = Random.Range(SpeedMin + (SpeedMax - SpeedMin) * 0.75f, SpeedMax);
            yield return new WaitForSeconds(Random.Range(ChangeSpeedFastSecondsMin, ChangeSpeedFastSecondsMax));
        }
    }

    private IEnumerator ChangePathDirs()
    {
        while (true)
        {
            yield return new WaitForSeconds(PathCfg.ChangeDirsSeconds);
            PathCfg.RandomlyChangeDirs();
        }
    }

    protected override Vector2 GetPositionByTimer(float timer)
    {
        var angle = Mathf.Lerp(0.0f, 2 * Mathf.PI, timer);
        var angleBasedDelta = new Vector2(PathCfg.GetX(angle), PathCfg.GetY(angle));
        return initialPosition + firstStepDelta + angleBasedDelta * FlyAreaScale;
    }

    private void RecalcFirstStepDelta()
    {
        var newDelta = initialPosition - GetPositionByTimer(0.0f) + firstStepDelta;
        firstStepDelta = newDelta;
    }
}