using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource musicSource;
    public AudioSource fxSource;

    public AudioClip waveClip;
    public AudioClip loseClip;
    public AudioClip winClip;
    public AudioClip batClip;
    public AudioClip hitBatClip;
    public AudioClip hitMothClip;

    public float pitchRandomness = 0.2f;
    private float originalFXPitch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        originalFXPitch = fxSource.pitch;
    }

    public static void PlayWave()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayWave();
    }

    private void InstancePlayWave()
    {
        PlayFxClip(waveClip);
    }

    public static void PlayLose()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayLose();
    }

    private void InstancePlayLose()
    {
        PlayFxClip(loseClip);
    }

    public static void PlayWin()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayWin();
    }

    private void InstancePlayWin()
    {
        PlayFxClip(winClip);
    }

    public static void PlayBat()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayBat();
    }

    private void InstancePlayBat()
    {
        PlayFxClip(batClip);
    }

    public static void PlayHitBat()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayHitBat();
    }

    private void InstancePlayHitBat()
    {
        PlayFxClip(hitBatClip);
    }

    public static void PlayHitMoth()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayHitMoth();
    }

    private void InstancePlayHitMoth()
    {
        PlayFxClip(hitMothClip);
    }

    private void PlayFxClip(AudioClip clip)
    {
        fxSource.pitch = originalFXPitch + Random.Range(-pitchRandomness, pitchRandomness);
        fxSource.PlayOneShot(clip);
    }
}