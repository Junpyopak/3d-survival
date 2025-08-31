using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenBlur : MonoBehaviour
{
    public Material blurMaterial;    // FPSIntroCutscene에서 할당
    [Range(0f, 10f)]
    public float blurSize = 0f;      // 현재 블러 강도

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (blurMaterial != null)
        {
            blurMaterial.SetFloat("_BlurSize", blurSize);
            Graphics.Blit(src, dest, blurMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
