using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class AnimationService
    {
        private IJSRuntime jsRuntime;

        public AnimationService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask LoadAnimation(string elementId, int width, int height)
        {
            return jsRuntime.InvokeVoidAsync("animatedBeam.loadAnimation", elementId, width, height);
        }
    }
}