using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityURP.Outline
{
    [ExecuteAlways]
    public class OutlineRenderer : MonoBehaviour
    {
        [SerializeField] private BlurredBufferOutlineRendererFeature outlineRendererFeature;
        [SerializeField] private List<Renderer> outlineRenderers;

        private void OnValidate()
        {
            if (outlineRenderers == null || outlineRenderers.Count == 0)
                FillRenderersNoParticleSystems();

            OnEnable();
        }

        [ContextMenu("Reassign Renderers")]
        public void FillAllRenderers()
        {
            outlineRenderers = GetComponentsInChildren<Renderer>(true).ToList();
        }

        [ContextMenu("Reassign Renderers without Particles")]
        public void FillRenderersNoParticleSystems()
        {
            var allRenderers = GetComponentsInChildren<Renderer>(true);
            outlineRenderers = new List<Renderer>(allRenderers.Length);

            foreach (var renderer in allRenderers)
            {
                if (!(renderer is ParticleSystemRenderer))
                {
                    outlineRenderers.Add(renderer);
                }
            }
        }

        private void OnEnable()
        {
            outlineRendererFeature?.AddRenderers(outlineRenderers);
        }

        private void OnDisable()
        {
            outlineRendererFeature?.RemoveRenderers(outlineRenderers);
        }
    }
}