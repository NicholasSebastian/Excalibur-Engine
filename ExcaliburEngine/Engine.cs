using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ExcaliburEngine
{
    public abstract class Engine : GameWindow
    {
        protected abstract void Start();
        protected abstract void Update();
        protected abstract void Render();
        protected abstract void End();

        public Engine(int width, int height, string title) :
            base(
                GameWindowSettings.Default, 
                new NativeWindowSettings() {
                    Size = new OpenTK.Mathematics.Vector2i(width, height),
                    Title = title
                })
        {
            VSync = VSyncMode.On;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            Shader.Initialize();
            Renderer.Initialize();
            Start();
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Update();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Renderer.Prepare();
            Shader.Use();
            Render();
            Shader.Detach();
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload()
        {
            Loader.Cleanup();
            Shader.Cleanup();
            End();
            base.OnUnload();
        }
    }
}
