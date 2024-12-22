using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class NoRendererButton : MaskableGraphic
{
    public event UnityAction OnPointClick;

    protected NoRendererButton()
    {
        useLegacyMeshGeneration = false;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }

    public void OnClick()
    {
        if (OnPointClick != null) OnPointClick();
    }
}
