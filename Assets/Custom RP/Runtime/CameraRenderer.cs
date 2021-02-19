using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    Camera camera;
    ScriptableRenderContext context;
    CullingResults cullingResults;

    // Create CommandBuffer
    const string bufferName = "Render Camera";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };

    // https://docs.unity3d.com/ScriptReference/Rendering.ShaderTagId.html
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    // Render Function
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer(); // It's clearer if each camera gets its own scope(Separate samples per camera)

        PrepareForSceneWindow(); // Draw UI

        if (!Cull()) // Culling
            return;

        Setup(); // Set up current camera

        DrawVisibleGeometry(); // Draw visible Geometries

        DrawUnsupportedShaders(); // Draw unsupported Geometries

        DrawGizmos(); // Draw Gizmos

        Submit(); // We have to submit the queued work for execution
    }

    void DrawVisibleGeometry()
    {
        //  1. Draw Qpaque Objects first
        // https://docs.unity3d.com/ScriptReference/Rendering.SortingSettings.html
        // https://docs.unity3d.com/ScriptReference/Rendering.SortingCriteria.html
        var sortingSettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };

        // https://docs.unity3d.com/ScriptReference/Rendering.DrawingSettings.html
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);

        // https://docs.unity3d.com/ScriptReference/Rendering.FilteringSettings.html
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.DrawRenderers.html
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        // 2. Draw Skybox
        context.DrawSkybox(camera);

        // 3. Draw Transparent objects after Skybox
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    void Setup()
    {
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.SetupCameraProperties.html
        context.SetupCameraProperties(camera);

        // https://docs.unity3d.com/ScriptReference/CameraClearFlags.html
        CameraClearFlags flags = camera.clearFlags;

        buffer.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);

        // https://docs.unity3d.com/ScriptReference/Rendering.CommandBuffer.BeginSample.html
        buffer.BeginSample(SampleName);

        ExecuteBuffer();
    }

    void Submit()
    {
        // https://docs.unity3d.com/ScriptReference/Rendering.CommandBuffer.EndSample.html
        buffer.EndSample(SampleName);

        ExecuteBuffer();

        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.Submit.html
        context.Submit();
    }

    void ExecuteBuffer()
    {
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.ExecuteCommandBuffer.html
        context.ExecuteCommandBuffer(buffer);

        buffer.Clear();
    }

    bool Cull()
    {
        // https://docs.unity3d.com/ScriptReference/Camera.TryGetCullingParameters.html
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters param))
        {
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.Cull.html
            cullingResults = context.Cull(ref param);
            return true;
        }

        return false;
    }
}
