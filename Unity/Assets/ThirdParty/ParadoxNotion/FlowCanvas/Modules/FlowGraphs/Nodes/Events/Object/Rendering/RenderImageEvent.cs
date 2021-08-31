using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace FlowCanvas
{
    [Name("On Render Image")]
    [Category("Events/Object/Rendering")]
    [Description("Called after all rendering is complete to render image on target camera.")]
    public class RenderImageEvent : RouterEventNode<Camera>
    {

        private FlowOutput fOut;
        private RenderTexture t1;
        private RenderTexture t2;

        protected override void Subscribe(EventRouter router) { router.onRenderImage += OnRenderImage; }
        protected override void UnSubscribe(EventRouter router) { router.onRenderImage -= OnRenderImage; }

        void OnRenderImage(RenderTexture source, RenderTexture dest) {
            t1 = source;
            t2 = dest;
            fOut.Call(new Flow());
        }

        protected override void RegisterPorts() {
            fOut = AddFlowOutput("Out");
            var t1Out = AddValueOutput<RenderTexture>("in", () => t1);
            var t2Out = AddValueOutput<RenderTexture>("out", () => t2);
        }
    }
}