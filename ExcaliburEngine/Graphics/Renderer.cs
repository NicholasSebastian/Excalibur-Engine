using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace ExcaliburEngine
{
    public static class Renderer
    {
        private static int positionLocation;
        private static int texturePositionLocation;

        internal static void Initialize() 
        {
            positionLocation = Shader.GetAttribLocation("aPosition");
            texturePositionLocation = Shader.GetAttribLocation("aTexturePosition");

            GL.ClearColor(Color.CornflowerBlue);
        }

        internal static void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public static void Render(Model model)
        {
            GL.BindVertexArray(model.vertexArrayObject);
            GL.EnableVertexAttribArray(positionLocation);
            GL.EnableVertexAttribArray(texturePositionLocation);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.texture.diffuse);

            GL.DrawElements(PrimitiveType.Triangles, model.indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.DisableVertexAttribArray(positionLocation);
            GL.DisableVertexAttribArray(texturePositionLocation);
            GL.BindVertexArray(0);
        }
    }
}
