using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ExcaliburEngine
{
    public static class Loader
    {
        private static readonly List<int> vaos = new List<int>();
        private static readonly List<int> buffers = new List<int>();
        private static readonly List<int> textures = new List<int>();

        public static Model Load(float[] vertices, uint[] indices, string texturePath)
        {
            int vertexArrayObject = CreateVAO();

            LoadData(vertices, indices);
            Texture texture = LoadTexture(texturePath);

            LoadToVAO();
            Unbind();
            return new Model(vertexArrayObject, indices, texture);
        }

        internal static void Cleanup()
        {
            // TODO: fix crash on Close().

            GL.DeleteBuffers(buffers.Count, buffers.ToArray());
            GL.DeleteTextures(textures.Count, textures.ToArray());
            GL.DeleteVertexArrays(vaos.Count, vaos.ToArray());
        }

        private static int CreateVAO()
        {
            // VAO: Stores buffer objects.
            int vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vaos.Add(vertexArrayObject);
            return vertexArrayObject;
        }

        private static void LoadData(float[] vertices, uint[] indices)
        {
            // VBO: Stores the actual vertex attribute data.
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            buffers.Add(vertexBufferObject);

            // EBO: Allows the same vertices to be reused.
            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            buffers.Add(elementBufferObject);
        }

        private static Texture LoadTexture(string texturePath)
        {
            // TODO: Texture just tiles horizontally for some reason.
            // TODO: Texture is monochromatic for some reason.

            int textureObject = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureObject);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Image<Rgba32> image = Image.Load<Rgba32>(texturePath);
            byte[] pixels = ParseImage(image);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            textures.Add(textureObject);
            return new Texture()
            {
                diffuse = textureObject
            };
        }

        private static void LoadToVAO()
        {
            const int positionLength = 3;
            const int textureLength = 2;
            const int totalLength = positionLength + textureLength;

            int positionLocation = Shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, positionLength, VertexAttribPointerType.Float, false, totalLength * sizeof(float), 0);

            int textureLocation = Shader.GetAttribLocation("aTexturePosition");
            GL.VertexAttribPointer(textureLocation, textureLength, VertexAttribPointerType.Float, false, totalLength * sizeof(float), positionLength * sizeof(float));
        }

        private static void Unbind()
        {
            // Unbind VBO, Texture, and VAO.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
        }

        private static byte[] ParseImage(Image<Rgba32> image)
        {
            List<Rgba32> pixels = new List<Rgba32>();
            for (int i = 0; i < image.Height; i++)
                pixels.AddRange(image.GetPixelRowSpan(i).ToArray());

            byte[] pixelBytes = new byte[pixels.Count * 4];
            for (int i = 0; i < pixels.Count; i++)
            {
                pixelBytes[i] = pixels[i].R;
                pixelBytes[i + 1] = pixels[i].G;
                pixelBytes[i + 2] = pixels[i].B;
                pixelBytes[i + 3] = pixels[i].A;
            }

            return pixelBytes;
        }
    }
}
