using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{

    public SpriteRenderer sr;

    public int resolution = 256;

    public int noiseResolution = 10;

    public Vector2 offset;

    public List<ColorSetter> colorSetters;

    Texture2D tex;

    // Start is called before the first frame update
    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();

        tex = new Texture2D(resolution, resolution, TextureFormat.RGBA64, 4, false);
        tex.filterMode = FilterMode.Point;

        FillTexture();
        ColorSet();

        sr.material.SetTexture("mainTexture", tex);
    }

    void Update()
    {

        if(tex.height != resolution)
        {
            tex.Resize(resolution, resolution);
        }

        if(tex.height != noiseResolution)
        {
            FillTexture();
            ColorSet();
        }   
    }

    // Update is called once per frame
    void FillTexture()
    {
        float step = 1f/resolution;
        for (int x = 0; x < resolution; x++)
        {        
            for (int y = 0; y < resolution; y++)
            {
                tex.SetPixel(x, y, Color.white * Mathf.PerlinNoise(x * step * noiseResolution + offset.x, y * step * noiseResolution + offset.y));
            }
        }
        tex.Apply();
    }

    void ColorSet()
    {
        float step = 1f/resolution;

        for(int x = 0; x < resolution; x++)
        {
            for(int y = 0; y < resolution; y++)
            {
                
                
                float h = 0;
                float s = 0;
                float v = 0;
                Color.RGBToHSV(tex.GetPixel(x, y), out h, out s, out v);
                

                foreach(ColorSetter colorSetter in colorSetters)
                {
                    if(v < colorSetter.maxValue && v > colorSetter.minValue)
                    {
                        tex.SetPixel(x, y, colorSetter.color);
                    }
                }
            }   
        }
        tex.Apply();
    }
}
