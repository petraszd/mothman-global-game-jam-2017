using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameplay : MonoBehaviour
{
    public float Timeout;

	protected virtual void Start ()
	{
	    StartCoroutine(PauseAndAllowLoading());
	}

    protected IEnumerator PauseAndAllowLoading()
    {
        yield return new WaitForSeconds(Timeout);
        BeforeAllowLoading();
        while (true)
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    protected virtual void BeforeAllowLoading()
    {
    }
}
