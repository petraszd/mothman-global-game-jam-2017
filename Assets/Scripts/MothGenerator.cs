using System.Collections;
using UnityEngine;

public class MothGenerator : PrefabPool
{
    public delegate void MothGeneratorStartEvent(int count);
    public delegate void MothGeneratorEndEvent();

    public static event MothGeneratorStartEvent OnStartGenerating;
    public static event MothGeneratorEndEvent OnAllReduced;

    public int PrefarableNumber;
    public float MothStartDelayMin;
    public float MothStartDelayMax;

    public float PaddingFraction;
    public float MaxWidth;

    private Vector2 viewportTop;
    private float lengthOfScreen;

    private int numberGenerated;

    private float GenAreaXMin;
    private float GenAreaXMax;
    private float GenAreaYMin;
    private float GenAreaYMax;

    private void OnEnable()
    {
        GameManager.OnLose += OnGameLose;
        GameManager.OnActiveMothReduced += OnActiveMothReduced;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= OnGameLose;
        GameManager.OnActiveMothReduced -= OnActiveMothReduced;
    }

    private void Start()
    {
        numberGenerated = 0;
        viewportTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.0f));
        lengthOfScreen = (
            Camera.main.ViewportToWorldPoint(Vector3.right) -
            Camera.main.ViewportToWorldPoint(Vector3.zero)
        ).x;

        SetGenAreaVariables();

        EmitStartGenerating();
        InitialGenerateMoths();
    }

#if UNITY_EDITOR

    private void Update()
    {
        var x0 = GenAreaXMin;
        var x1 = GenAreaXMax;

        var y0 = GenAreaYMin;
        var y1 = GenAreaYMax;

        Debug.DrawLine(new Vector3(x0, y1), new Vector3(x1, y1));
        Debug.DrawLine(new Vector3(x1, y1), new Vector3(x1, y0));
        Debug.DrawLine(new Vector3(x1, y0), new Vector3(x0, y0));
        Debug.DrawLine(new Vector3(x0, y0), new Vector3(x0, y1));
    }

#endif

    private void SetGenAreaVariables()
    {
        var middleLeft = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.5f));
        var topRight = Camera.main.ViewportToWorldPoint(Vector2.one);

        var maxX = MaxWidth / 2.0f;

        GenAreaXMin = Mathf.Max(-maxX, middleLeft.x);
        GenAreaXMax = Mathf.Min(maxX, topRight.x);

        GenAreaYMin = middleLeft.y;
        GenAreaYMax = topRight.y;

        var padding = (GenAreaXMax - GenAreaXMin) * PaddingFraction;
        GenAreaXMin += padding;
        GenAreaXMax -= padding;

        GenAreaYMin += padding;
        GenAreaYMax -= padding;
    }

    private void InitialGenerateMoths()
    {
        for (var i = 0; i < PrefarableNumber; ++i)
        {
            StartCoroutine(GenerateOneMothDelayed(GetStartPositionByIndex(i)));
        }
    }

    private IEnumerator GenerateOneMothDelayed(Vector2 start)
    {
        yield return new WaitForSeconds(Random.Range(MothStartDelayMin, MothStartDelayMax));
        GenerateOneMoth(start);
    }

    private void GenerateOneMoth(Vector2 start)
    {
        if (numberGenerated == PoolSize)
        {
            return;
        }
        var moth = GetFromPool();
        numberGenerated++;
        var flyInto = moth.GetComponent<MothFlyTo>();
        flyInto.StartPosition = start;
        flyInto.EndPosition = GenerateRandomEnd();
        flyInto.enabled = true;
    }

    private Vector2 GenerateRandomEnd()
    {
        return new Vector2(Random.Range(GenAreaXMin, GenAreaXMax), Random.Range(GenAreaYMin, GenAreaYMax));
    }

    private Vector2 GetStartPositionByIndex(int index)
    {
        var amount = Mathf.Lerp(0.0f, Mathf.PI, index / (float) (PrefarableNumber - 1));
        var delta = new Vector2(Mathf.Cos(amount), Mathf.Sin(amount));
        return viewportTop + delta * lengthOfScreen;
    }

    private void OnActiveMothReduced(int currentCount)
    {
        if (currentCount < PrefarableNumber)
        {
            var index = Random.Range(0, PrefarableNumber);
            StartCoroutine(GenerateOneMothDelayed(GetStartPositionByIndex(index)));
        }

        if (currentCount == 0 && numberGenerated >= PoolSize)
        {
            EmitAllReduced();
        }
    }

    private void EmitAllReduced()
    {
        if (OnAllReduced != null)
        {
            OnAllReduced();
        }
    }

    private void OnGameLose()
    {
        Destroy(gameObject);
    }

    private void EmitStartGenerating()
    {
        if (OnStartGenerating != null)
        {
            OnStartGenerating(PoolSize);
        }
    }
}
