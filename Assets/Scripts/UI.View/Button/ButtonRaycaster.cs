using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Button
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class ButtonRaycaster : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}