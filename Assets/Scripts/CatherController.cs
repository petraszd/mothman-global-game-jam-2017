using UnityEngine;

public class CatherController : MonoBehaviour
{
    private enum FingerState
    {
        Still,
        Ready,
        Pressed,
        Shooting
    }

    public GameObject WaveActive;
    public GameObject WaveInactive;
    public GameObject FingerActive;

    public float RotateSpeed;

    private Rigidbody2D waveRb2D;
    private FingerState state;
    private Vector2 inputStartPosition;
    private Animator anim;

    private void Awake()
    {
        waveRb2D = WaveActive.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        state = FingerState.Still;
        anim.SetTrigger("Charge");
    }

    private void OnEnable()
    {
        GameManager.OnLose += OnGameLose;
        GameManager.OnWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= OnGameLose;
        GameManager.OnWin -= OnGameWin;
    }

    private void Update()
    {
        if (state == FingerState.Ready)
        {
            ProccessOnReady();
        }
        if (state == FingerState.Pressed)
        {
            ProccessOnPressed();
        }
    }

    public void OnShootOver()
    {
        state = FingerState.Still;
        WaveActive.SetActive(false);
        FingerActive.SetActive(false);
        WaveInactive.transform.eulerAngles = Vector3.zero;
        anim.SetTrigger("Charge");
    }

    public void OnChargeOver()
    {
        state = FingerState.Ready;
    }

    private void ProccessOnReady()
    {
        if (GetInputDown())
        {
            FingerActive.SetActive(true);
            inputStartPosition = GetInputPosition();
            WaveInactive.SetActive(true);
            state = FingerState.Pressed;
        }
    }

    private void ProccessOnPressed()
    {
        if (GetInputUp())
        {
            WaveInactive.SetActive(false);
            WaveActive.SetActive(true);
            WaveActive.transform.eulerAngles = WaveInactive.transform.eulerAngles;
            waveRb2D.MoveRotation(WaveInactive.transform.eulerAngles.z);
            state = FingerState.Shooting;
            anim.SetTrigger("Shoot");
            AudioManager.PlayWave();
        }
        else
        {
            var mouseDelta = (GetInputPosition() - inputStartPosition).x;
            var angleDelta = 0.0f;
            if (mouseDelta < 0.0f)
            {
                angleDelta = RotateSpeed * Time.deltaTime;
            }
            else if (mouseDelta > 0.0f)
            {
                angleDelta = -RotateSpeed * Time.deltaTime;
            }

            if (angleDelta != 0.0f)
            {
                WaveInactive.transform.Rotate(0.0f, 0.0f, angleDelta);
            }
        }
    }

    private bool GetInputDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool GetInputUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    private Vector2 GetInputPosition()
    {
        return Input.mousePosition;
    }

    private void OnGameLose()
    {
        DisableSelf();
    }

    private void OnGameWin()
    {
        DisableSelf();
    }

    private void DisableSelf()
    {
        anim.SetTrigger("Still");
        state = FingerState.Still;
        WaveActive.SetActive(false);
        WaveInactive.SetActive(false);
        FingerActive.SetActive(false);
    }
}