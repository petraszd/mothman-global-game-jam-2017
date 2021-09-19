using UnityEngine;

[System.Serializable]
public struct MothFlyingPathCFG
{
    public float EllipseXMin;
    public float EllipseXMax;
    public float EllipseYMin;
    public float EllipseYMax;
    public float AmplitudeXMin;
    public float AmplitudeXMax;
    public float AmplitudeYMin;
    public float AmplitudeYMax;
    public int SinusiodCountMin;
    public int SinusiodCountMax;

    public float EllipseSpeedMax;
    public float AmplitudeSpeedMax;
    public float AngleSpeedMax;

    public float ChangeDirsSeconds;

    public float EllipseX;
    public float EllipseY;
    public float AmplitudeX;
    public float AmplitudeY;
    public float AngleX;
    public float AngleY;
    public int SinusiodCount;

    private float ellipseXDir;
    private float ellipseYDir;
    private float amplitudeXDir;
    private float amplitudeYDir;
    private float angleXDir;
    private float angleYDir;

    public float GetX(float angle)
    {
        return (EllipseX + AmplitudeX * Mathf.Sin(SinusiodCount * angle)) * Mathf.Cos(AngleX + angle);
    }

    public float GetY(float angle)
    {
        return (EllipseY + AmplitudeY * Mathf.Sin(SinusiodCount * angle)) * Mathf.Sin(AngleY + angle);
    }

    public void ResetWithRandomValues()
    {
        EllipseX = Random.Range(EllipseXMin, EllipseXMax);
        EllipseY = Random.Range(EllipseYMin, EllipseYMax);
        AmplitudeX = Random.Range(AmplitudeXMin, AmplitudeXMax);
        AmplitudeY = Random.Range(AmplitudeYMin, AmplitudeYMax);
        AngleX = 0.0f;
        AngleY = 0.0f;
        SinusiodCount = Random.Range(SinusiodCountMin, SinusiodCountMax);
    }

    public void SlowlyChange()
    {
        EllipseX += ellipseXDir * Random.Range(0.0f, EllipseSpeedMax) * Time.deltaTime;
        EllipseY += ellipseYDir * Random.Range(0.0f, EllipseSpeedMax) * Time.deltaTime;
        AmplitudeX += amplitudeXDir * Random.Range(0.0f, AmplitudeSpeedMax) * Time.deltaTime;
        AmplitudeY += amplitudeYDir * Random.Range(0.0f, AmplitudeSpeedMax) * Time.deltaTime;
        AngleX += angleXDir * Random.Range(0.0f, AngleSpeedMax) * Time.deltaTime;
        AngleY += angleYDir * Random.Range(0.0f, AngleSpeedMax) * Time.deltaTime;
    }

    public void RandomlyChangeDirs()
    {
        ellipseXDir = Random.value < 0.5f ? 1.0f : -1.0f;
        ellipseYDir = Random.value < 0.5f ? 1.0f : -1.0f;
        amplitudeXDir = Random.value < 0.5f ? 1.0f : -1.0f;
        amplitudeYDir = Random.value < 0.5f ? 1.0f : -1.0f;
        angleXDir = Random.value < 0.5f ? 1.0f : -1.0f;
        angleYDir = Random.value < 0.5f ? 1.0f : -1.0f;
    }
}