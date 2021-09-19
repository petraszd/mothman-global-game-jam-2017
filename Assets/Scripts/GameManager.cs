using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void GameEndEvent();
    public delegate void MothCountEvent(int currentCount);

    public static event GameEndEvent OnWin;
    public static event GameEndEvent OnLose;
    public static event MothCountEvent OnActiveMothReduced;

    private enum EndCondition
    {
        NoEnd,
        Win,
        Lose
    }

    public float TimeoutBeforeWin;

    private int activeMothCount;
    private EndCondition endCondition;

    private void Start()
    {
        endCondition = EndCondition.NoEnd;
        activeMothCount = 0;
    }

    private void OnEnable()
    {
        MothState.OnHitByWave += OnMothHitByWave;
        MothState.OnBecomeActive += OnBecomeActive;
        MothGenerator.OnAllReduced += OnAllReduced;
        Background.OnBackgroundAnimationOver += OnTimeOver;
    }

    private void OnDisable()
    {
        MothState.OnHitByWave -= OnMothHitByWave;
        MothState.OnBecomeActive -= OnBecomeActive;
        MothGenerator.OnAllReduced -= OnAllReduced;
        Background.OnBackgroundAnimationOver -= OnTimeOver;
    }

    private void OnMothHitByWave()
    {
        activeMothCount--;
        EmitActiveMothReduced();
    }

    private void OnBecomeActive()
    {
        activeMothCount++;
    }

    private void OnTimeOver()
    {
        if (endCondition == EndCondition.NoEnd)
        {
            endCondition = EndCondition.Lose;
            AudioManager.PlayLose();
            EmitLose();
        }
    }

    private void OnAllReduced()
    {
        if (endCondition == EndCondition.NoEnd)
        {
            endCondition = EndCondition.Win;
            AudioManager.PlayWin();
            EmitWin();
            StartCoroutine(LoadWinDelayed());
        }
    }

    private IEnumerator LoadWinDelayed()
    {
        yield return new WaitForSeconds(TimeoutBeforeWin);
        SceneManager.LoadScene("Win");
    }

    private void EmitActiveMothReduced()
    {
        if (OnActiveMothReduced != null)
        {
            OnActiveMothReduced(activeMothCount);
        }
    }

    private void EmitWin()
    {
        if (OnWin != null)
        {
            OnWin();
        }
    }

    private void EmitLose()
    {
        if (OnLose != null)
        {
            OnLose();
        }
    }
}
