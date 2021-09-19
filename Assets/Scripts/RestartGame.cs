using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : LoadGameplay {
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        GameManager.OnLose += OnGameLose;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= OnGameLose;
    }

    protected override void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnGameLose()
    {
        StartCoroutine(PauseAndAllowLoading());
    }

    protected override void BeforeAllowLoading()
    {
        spriteRenderer.enabled = true;
    }
}
