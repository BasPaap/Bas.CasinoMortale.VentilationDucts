namespace UnityEngine.Rendering.Universal
{
    public enum BufferType
    {
        CameraColor,
        Custom
    }

    public class DrawFullscreenFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class RenderFeatureSettings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

            public Material blitMaterial = null;
            public int blitMaterialPassIndex = -1;
            public BufferType sourceType = BufferType.CameraColor;
            public BufferType destinationType = BufferType.CameraColor;
            public string sourceTextureId = "_SourceTexture";
            public string destinationTextureId = "_DestinationTexture";
        }

        public RenderFeatureSettings renderFeatureSettings = new RenderFeatureSettings();
        DrawFullscreenPass blitPass;

        public override void Create()
        {
            blitPass = new DrawFullscreenPass(name);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderFeatureSettings.blitMaterial == null)
            {
                Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }
            else if (!Settings.Instance.IsRetroEffectEnabled && renderFeatureSettings.blitMaterial.name == "Retro Screen")
            {
                return;
            }

            blitPass.renderPassEvent = renderFeatureSettings.renderPassEvent;
            blitPass.settings = renderFeatureSettings;
            renderer.EnqueuePass(blitPass);
        }
    }
}
