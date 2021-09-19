using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public delegate void BatEvent();

    public static event BatEvent OnBatHit;

    public float CooldownInSeconds;
    public Transform ParticleHolder;
    private bool isCooldown;
    private ParticleSystem partices;

    private void OnEnable()
    {
        GameManager.OnWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameManager.OnWin -= OnGameWin;
    }

    private void Start()
    {
        isCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCooldown && other.tag == "Catcher")
        {
            PlayEffects();
            AudioManager.PlayHitBat();
            StartCoroutine(WaitForCooldown());
            EmitBatHit();
        }
    }

    public void MaybePlayAudio()
    {
        if (Random.value < 0.4f)
        {
            AudioManager.PlayBat();
        }

    }

    private IEnumerator WaitForCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(CooldownInSeconds);
        isCooldown = false;
    }

    private void EmitBatHit()
    {
        if (OnBatHit != null)
        {
            OnBatHit();
        }
    }

    private void PlayEffects()
    {
        ParticleHolder.transform.position = transform.position;
        ParticleHolder.GetComponent<ParticleSystem>().Play();
    }

    private void OnGameWin()
    {
        PlayEffects();
        Destroy(gameObject);
    }
}