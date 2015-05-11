using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SteeringEntities
{
    public class TestingShadersState : GameState
    {
        VertexPositionColorTexture[] vertices;
        int[] indices;
        Matrix viewMat, projMat;
        BasicEffect basicEffect;
        //Effect blurEffect;
        //RenderTarget2D renderTarget;
        //bool useBlur = false;

        PerlinNoise pNoise = new PerlinNoise();
        VertexPositionColorTexture[] noiseVerts;
        int[] noiseIndices;
        int rows = 2;
        int cols = 2;

        public TestingShadersState()
            : base("TestingShaderState")
        {
            vertices = new VertexPositionColorTexture[4];

            vertices[0].Position = new Vector3(-0.5f, -0.5f, 0);
            vertices[0].TextureCoordinate = new Vector2(0, 1);
            vertices[0].Color = new Color((float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255));

            vertices[1].Position = new Vector3(-0.5f, 0.5f, 0);
            vertices[1].TextureCoordinate = new Vector2(0, 0);
            vertices[1].Color = new Color((float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255));

            vertices[2].Position = new Vector3(0.5f, -0.5f, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 1);
            vertices[2].Color = new Color((float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255));

            vertices[3].Position = new Vector3(0.5f, 0.5f, 0);
            vertices[3].TextureCoordinate = new Vector2(1, 0);
            vertices[3].Color = new Color((float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255),
                                          (float)pNoise.Noise(Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble(), Game1.cryptoRand.NextDouble() * 255));

            indices = new int[6];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 3;

            int vertCount = rows * cols;
            noiseVerts = new VertexPositionColorTexture[vertCount];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    noiseVerts[(i * cols) + j].Position = new Vector3(j, i, 0);
                    noiseVerts[(i * cols) + j].Color = Game1.RandomColour();
                    noiseVerts[(i * cols) + j].TextureCoordinate = new Vector2(j / (float)(cols - 1), i / (float)(rows - 1));
                }
            }

            int indicesCount = (rows - 1) * (cols - 1) * 6;
            int index = 0;

            noiseIndices = new int[indicesCount];

            for (int i = 0; i < rows - 1; ++i)
            {
                for (int j = 0; j < cols - 1; ++j)
                {
                    noiseIndices[index++] = i * cols + j;
                    noiseIndices[index++] = i * cols + (j + 1);
                    noiseIndices[index++] = (i + 1) * cols + j;

                    noiseIndices[index++] = (i + 1) * cols + j;
                    noiseIndices[index++] = i * cols + (j + 1);
                    noiseIndices[index++] = (i + 1) * cols + j + 1;
                }
            }

            viewMat = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            projMat = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);
            basicEffect = new BasicEffect(GameStateManager.Instance.GfxDevice);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = viewMat;
            basicEffect.Projection = projMat;
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.Texture = m_Content.Load<Texture2D>("2048");

            //Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SteeringEntities.Content.blurShader.mgfxo");
            //using (BinaryReader br = new BinaryReader(s))
            //{
            //    blurEffect = new Effect(GameStateManager.Instance.GfxDevice, br.ReadBytes((int)br.BaseStream.Length));
            //    //blurEffect = m_Content.Load<Effect>("blurShader");
            //}
            //renderTarget = new RenderTarget2D(GameStateManager.Instance.GfxDevice, GameStateManager.Instance.GfxDevice.PresentationParameters.BackBufferWidth, GameStateManager.Instance.GfxDevice.PresentationParameters.BackBufferHeight, false, GameStateManager.Instance.GfxDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw()
        {
            //if (useBlur)
            //{
            //    GameStateManager.Instance.GfxDevice.SetRenderTarget(renderTarget);
            //}

            base.Draw();

            foreach (EffectPass n in basicEffect.CurrentTechnique.Passes)
            {
                n.Apply();
                //GameStateManager.Instance.GfxDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
                GameStateManager.Instance.GfxDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, noiseVerts, 0, 4, noiseIndices, 0, 2);
            }

            //if (useBlur)
            //{
            //    GameStateManager.Instance.GfxDevice.SetRenderTarget(null);
            //    GameStateManager.Instance.GfxDevice.Clear(Color.Black);

            //    blurEffect.CurrentTechnique.Passes[0].Apply();
            //    Game1.sb.Draw(renderTarget, Vector2.Zero, Color.White);
            //}
        }
    }
}
