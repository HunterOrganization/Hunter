using UnityEngine;
using System.Collections;

[ExecuteInEditMode] // 编辑状态下也能看到效果
[RequireComponent(typeof(Camera))] // 自动绑定
public class PostEffectsBase : MonoBehaviour
{

    void Start()
    {
        CheckResource();
    }

    #region 检查平台
    protected void CheckResource()
    {
        // 检查资源和条件是否满足
        bool isSupported = CheckSuppoert();

        if (isSupported == false)
            NotSupported();
    }

    protected bool CheckSuppoert()
    {
        if (SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false)
        {
            Debug.LogWarning("This platform does not support image effects or render texture");
            return false;
        }
        return true;
    }

    protected void NotSupported()
    {
        enabled = false;
    }
    #endregion

    // 指定Shader创建用于处理渲染纹理的材质
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null || !shader.isSupported)
            return null;

        if (shader.isSupported && material && material.shader == shader)
            return material;

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        else
            return null;
    }
}
