using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace LKZ.TransparentWindow
{
    public class TransparentWindowRendererFeature : ScriptableRendererFeature
    {
        public RenderPassEvent m_CopyColorEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material blitMaterial;
         
        public class Pass : ScriptableRenderPass
        { 
            private Material m_Material;
             
            public Pass(RenderPassEvent evt, Material mat)
            {
                base.renderPassEvent = evt;
                this.m_Material = mat; 

            }

            public void SetInput(RTHandle src)
            {
                m_Material.SetTexture("_MainTex", src.rt);
            }
             
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            { 
                CommandBuffer cmd = CommandBufferPool.Get();
                {
                    Blit(cmd, ref renderingData, m_Material);
                }
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType != CameraType.Game)
                return;
             
            renderer.EnqueuePass(pass);


        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType != CameraType.Game)
                return;
             
            pass.SetInput(renderer.cameraColorTargetHandle);
        }

        Pass pass;
        public override void Create()
        { 
            pass = new Pass(m_CopyColorEvent, blitMaterial);
        }
    }
}
