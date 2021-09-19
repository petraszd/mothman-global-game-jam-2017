using UnityEngine;

public class MothFlyTo : MothState
{
    private const int DEBUG_DRAW_COUNT = 20;

    public float Speed;

    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public MothState NextState;
    public bool IsHittable;

    private Vector2 perp;
    private Rigidbody2D rb2D;
    private float flyingTimer;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        NextState = GetComponent<MothFlyingAround>();
    }

    private void Start()
    {
        EmitOnBecomeActive();
        IsHittable = true;
        ResetToStart();
    }

    protected override void Update()
    {
        base.Update();

        flyingTimer += Time.deltaTime * Speed;

        if (flyingTimer > 1.0f)
        {
            IsHittable = false;
            enabled = false;
            NextState.enabled = true;
        }
    }

    public void ResetToStart()
    {
        flyingTimer = 0.0f;
        transform.position = StartPosition;
        var delta = EndPosition - StartPosition;
        perp = new Vector2(delta.y, -delta.x);
        perp.Normalize();
        if (Random.value < 0.5f)
        {
            perp *= -1.0f;
        }
    }

    public void ResetToStart(Vector2 start, Vector2 end, MothState next)
    {
        StartPosition = start;
        EndPosition = end;
        NextState = next;
        ResetToStart();
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(GetPositionByTimer(flyingTimer));
        rb2D.MoveRotation(GetAngleByTimer(flyingTimer));
    }

    protected override Vector2 GetPositionByTimer(float timer)
    {
        var angle = Mathf.Sin(Mathf.Lerp(0.0f, Mathf.PI, timer));
        return Vector2.Lerp(StartPosition + perp * angle, EndPosition + perp * angle, timer);
    }
}