using UnityEngine;
using UnityEngine.UI;

public class NumberRemainingUI : MonoBehaviour
{
    private Text text;

    private int totalCount;
    private int onBeardCount;
    private bool isEnd;

    private void OnEnable()
    {
        GameManager.OnLose += OnGameEnd;
        GameManager.OnWin += OnGameEnd;

        MothGenerator.OnStartGenerating += OnTotalNumber;

        MothState.OnSitOnBeard += OnIncBeardNumber;
        MothState.OnFlyOffBeard += OnDecBeardNumber;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= OnGameEnd;
        GameManager.OnWin -= OnGameEnd;

        MothGenerator.OnStartGenerating -= OnTotalNumber;

        MothState.OnSitOnBeard -= OnIncBeardNumber;
        MothState.OnFlyOffBeard -= OnDecBeardNumber;
    }

    private void Awake()
    {
        totalCount = 0;
        onBeardCount = 0;

        isEnd = false;

        text = GetComponent<Text>();
        text.text = "";
    }

    private void OnGameEnd()
    {
        isEnd = true;
    }

    private void OnTotalNumber(int total)
    {
        totalCount = total;
        UpdateRemainingText();
    }

    private void UpdateRemainingText()
    {
        if (!isEnd && text != null)
        {
            text.text = "" + (totalCount - onBeardCount);
        }
    }

    private void OnIncBeardNumber()
    {
        onBeardCount++;
        UpdateRemainingText();
    }

    private void OnDecBeardNumber()
    {
        onBeardCount--;
        UpdateRemainingText();
    }
}
