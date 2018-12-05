using UnityEngine;

[System.Serializable]
public struct LightSource
{
    public Vector2 pos;
    public float range;
}

[ExecuteInEditMode]
public class LightCamera : MonoBehaviour {

    public Material lightMapper;
    public Material lightApplier;
    public LightSource[] lightSources;

    Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture lightMap = new RenderTexture(source.width, source.height, 16);

        for (int i = 0; i < lightSources.Length; i++)
        {
            RenderTexture tempMap = new RenderTexture(source.width, source.height, 16);

            Vector2 screenPoint = cam.WorldToScreenPoint(lightSources[i].pos);

            lightMapper.SetVector("_LightCoord", new Vector2(screenPoint.x, screenPoint.y));
            lightMapper.SetFloat("_SquaredLightRange", lightSources[i].range * lightSources[i].range);


            Graphics.Blit(lightMap, tempMap, lightMapper);
            lightMap = tempMap;
        }

        lightApplier.SetTexture("_LightMap", lightMap);
        Graphics.Blit(source, destination, lightApplier);
    }

}
