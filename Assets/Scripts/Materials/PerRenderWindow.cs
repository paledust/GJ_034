using UnityEngine;

public class PerRenderWindow : PerRendererBehavior
{
    [SerializeField] private Texture worldTex;
    private const string WORLD_TEX = "_WorldTex";
    protected override void InitProperties()
    {
        base.InitProperties();
        mpb.SetTexture(WORLD_TEX, worldTex);
    }
    protected override void UpdateProperties()
    {
        base.UpdateProperties();
        mpb.SetTexture(WORLD_TEX, worldTex);
    }
}
