using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Beam.Animation.Services
{
    public class AnimationService
    {
        private IJSRuntime jsRuntime;

        public static event Action BeamPassTriggered;

        public AnimationService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask LoadAnimation(string elementId, int width, int height)
        {
            return jsRuntime.InvokeVoidAsync("animatedBeam.loadAnimation", elementId, width, height);
        }

        [JSInvokable]
        public static Task BeamPassedBy()
        {
            return Task.Run(() => BeamPassTriggered?.Invoke());
        }
    }
}