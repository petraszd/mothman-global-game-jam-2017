using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public delegate void BakcgroundEvent();
    public static event BakcgroundEvent OnBackgroundAnimationOver;

    public void OnSlideOver()
    {
        EmitBackgroundAnimationOver();
    }

    private void EmitBackgroundAnimationOver()
    {
        if (OnBackgroundAnimationOver != null)
        {
            OnBackgroundAnimationOver();
        }
    }
}
