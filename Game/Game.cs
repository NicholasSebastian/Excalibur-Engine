using System;
using System.IO;
using ExcaliburEngine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game
{
    class Game : Engine
    {
        private const string title = "Excalibur Engine";
        private const int
            defaultWidth = 1280,
            defaultHeight = 720;

        public Game() : base(defaultWidth, defaultHeight, title) { }

        private float[] vertices =
        {   // Position             // TexturePosition
            -0.5f, 0.5f, 0.0f,      0.0f, 0.0f,         // top left         0
             0.5f, 0.5f, 0.0f,      1.0f, 0.0f,         // top right        1
            -0.5f, -0.5f, 0.0f,     0.0f, 1.0f,         // bottom left      2
             0.5f, -0.5f, 0.0f,     1.0f, 1.0f          // bottom right     3
        };

        private uint[] elementBuffer =
        {
            3, 1, 0,    // top right triangle
            0, 2, 3     // bottom left triangle
        };

        private Model rectangle;

        protected override void Start()
        {
            string texturePath = Path.Combine("Textures", "IceCream.jpg");

            rectangle = Loader.Load(vertices, elementBuffer, texturePath);
        }

        protected override void Update()
        {
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                Console.WriteLine("Space Key pressed.");
            }
        }

        protected override void Render()
        {
            Renderer.Render(rectangle);
        }

        protected override void End()
        {
            
        }
    }
}
