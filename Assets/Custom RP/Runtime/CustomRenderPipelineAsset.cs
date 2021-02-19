using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
	// RenderPipelineAsset:
	// https://docs.unity3d.com/ScriptReference/Rendering.RenderPipelineAsset.html

	protected override RenderPipeline CreatePipeline()
	{
		// 우리가 제작한 CustomRenderPipeline 클래스를 생성함
		return new CustomRenderPipeline();
	}
}