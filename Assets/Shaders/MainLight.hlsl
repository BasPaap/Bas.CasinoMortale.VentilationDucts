void MainLight_half(float3 WorldPosition, out half3 Direction, out half3 Color, out half DistanceAttenuation, out half ShadowAttenuation)
{
#ifdef SHADERGRAPH_PREVIEW
	Direction = half3(0.5, 0.5, 0);
	Color = 1;
	DistanceAttenuation = 1;
	ShadowAttenuation = 1;
#else
#ifdef SHADOWS_SCREEN
	half4 clipPosition = TransformWorldToHClip(WorldPosition);
	half4 shadowCoord = ComputeScreenPos(clipPosition);
#else
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
#endif
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAttenuation = mainLight.distanceAttenuation;
	ShadowAttenuation = mainLight.shadowAttenuation;
#endif
}