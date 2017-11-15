using UnityEngine;

[ExecuteInEditMode]
public class RenderWithShader : MonoBehaviour {

    public Material material;

    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            float fogValue = material.GetFloat("_depth") + 10 * Time.deltaTime;

            material.SetFloat("_depth", fogValue);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            float fogValue = material.GetFloat("_depth") - 10 * Time.deltaTime;

            material.SetFloat("_depth", fogValue);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}
