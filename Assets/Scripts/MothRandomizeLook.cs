using UnityEngine;

public class MothRandomizeLook : MonoBehaviour
{
    public float MinScale;
    public float MaxScale;

    public float MaxColorSue;

    public SpriteRenderer Sprite;

    private void Start()
    {
        var scale = Random.Range(MinScale, MaxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        Sprite.color = Color.HSVToRGB(Random.value, Random.Range(0.0f, MaxColorSue), 1.0f);

        enabled = false;
    }
}
