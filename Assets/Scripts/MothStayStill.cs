using UnityEngine;

public class MothStayStill : MothState
{
    public Animator Animator;
    public Vector2 PreviousAroundPosition;

    private Rigidbody2D rb2D;
    private MothFlyTo flyTo;
    private MothFlyingAround flyAround;

    protected override void OnEnable()
    {
        base.OnEnable();
        Bat.OnBatHit += OnBatHit;
        EmitSitOnBeard();
        if (Animator != null)
        {
            Animator.SetBool("isStill", true);
        }
    }

    protected override void OnDisable()
    {
        Bat.OnBatHit -= OnBatHit;
        base.OnDisable();
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        flyTo = GetComponent<MothFlyTo>();
        flyAround = GetComponent<MothFlyingAround>();
    }

    protected override Vector2 GetPositionByTimer(float timer)
    {
        return rb2D.position;
    }

    private void OnBatHit()
    {
        EmitOnBecomeActive();
        Animator.SetBool("isStill", false);
        EmitFlyOffBeard();
        enabled = false;
        flyAround.ResetAfterStayingStill();
        flyTo.ResetToStart(rb2D.position, PreviousAroundPosition, flyAround);
        flyTo.enabled = true;
    }
}
