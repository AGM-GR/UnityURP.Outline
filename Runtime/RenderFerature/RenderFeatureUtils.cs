using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityURP.Outline
{
    public static class RenderFeatureUtils
    {
        public static Material CreateMaterial(Shader materialShader)
        {
            return new Material(materialShader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }

        public static void Destroy(Object obj)
        {
            if (obj != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying && !EditorApplication.isPaused)
                {
                    Object.Destroy(obj);
                }
                else
                {
                    Object.DestroyImmediate(obj);
                }
#else
                Object.Destroy(obj);
#endif
            }
        }
    }
}
