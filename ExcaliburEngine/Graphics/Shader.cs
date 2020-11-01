using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace ExcaliburEngine
{
    internal static class Shader
    {
        private static int shaderProgram;

        private static readonly string vertexPath = Path.Combine("Shaders", "Shader.vert");
        private static readonly string fragmentPath = Path.Combine("Shaders", "Shader.frag");

        internal static void Initialize()
        {
            string vertexShaderSource = LoadFromFile(vertexPath); 
            string fragmentShaderSource = LoadFromFile(fragmentPath);

            // Compiling.
            int vertexShader = CompileShader(vertexShaderSource, ShaderType.VertexShader);
            int fragmentShader = CompileShader(fragmentShaderSource, ShaderType.FragmentShader);

            // Linking.
            shaderProgram = GL.CreateProgram();

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            // Cleanup.
            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        internal static void Use()
        {
            GL.UseProgram(shaderProgram);
        }

        internal static void Detach()
        {
            GL.UseProgram(0);
        }

        internal static void Cleanup()
        {
            GL.DeleteProgram(shaderProgram);
        }

        internal static int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(shaderProgram, attribName);
        }

        private static string LoadFromFile(string shaderPath)
        {
            using StreamReader reader = new StreamReader(shaderPath, Encoding.UTF8);
            string shaderSource = reader.ReadToEnd();
            return shaderSource;
        }

        private static int CompileShader(string shaderSource, ShaderType shaderType)
        {
            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            string compileStatus = GL.GetShaderInfoLog(shader);
            Console.WriteLine(
                compileStatus != string.Empty ? 
                compileStatus : 
                string.Format("{0} compiled successfully.", shaderType.ToString()));

            return shader;
        }
    }
}
