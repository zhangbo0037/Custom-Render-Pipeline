using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// RenderPipeline:
// https://docs.unity3d.com/ScriptReference/Rendering.RenderPipeline.html

// Renderer:
// https://docs.unity3d.com/ScriptReference/Renderer.html

// ScriptableRenderContext:
// https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html

public class CustomRenderPipeline : RenderPipeline
{
    CameraRenderer renderer = new CameraRenderer();

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach(Camera camera in cameras)
        {
            renderer.Render(context, camera);
        }
    }
}