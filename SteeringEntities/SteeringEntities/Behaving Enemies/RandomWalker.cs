using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class RandomWalker : Enemy
    {
        int negDir = 1;

        float totalTime = 0;
        float movementTimer = 0;
        float movementReady = 0.1f;

        public RandomWalker(Vector2 a_Position, Texture2D a_Texture)
            : base(a_Texture, a_Position, 125)
        {
            m_Colour = Color.Red;
            m_Velocity = Game1.RandomDirection();
        }

        public override void Update(GameTime gt)
        {
            movementTimer += (float)(gt.ElapsedGameTime.TotalSeconds);
            totalTime += (float)(gt.ElapsedGameTime.TotalSeconds);

            if (movementTimer > movementReady)
            {
                movementTimer = 0;

                if (Game1.cryptoRand.NextDouble() > 0.5)
                {
                    negDir = -negDir;
                }

                float x = ((float)Game1.perlinNoise.Noise(2 * m_Position.X * (1 / m_Texture.Width), 2 * m_Position.X * (1 / m_Texture.Width), -.5f) +
                          (float)Game1.perlinNoise.Noise(4 * m_Position.X * (1 / m_Texture.Width), 4 * m_Position.X * (1 / m_Texture.Width), 0) +
                          (float)Game1.perlinNoise.Noise(8 * m_Position.X * (1 / m_Texture.Width), 8 * m_Position.X * (1 / m_Texture.Width), -.5f)) * MonteCarloCustomDistribution() * negDir;

                float y = ((float)Game1.perlinNoise.Noise(2 * m_Position.Y * (1 / m_Texture.Height), 2 * m_Position.Y * (1 / m_Texture.Height), .5f) +
                          (float)Game1.perlinNoise.Noise(4 * m_Position.Y * (1 / m_Texture.Height), 4 * m_Position.Y * (1 / m_Texture.Height), 0) +
                          (float)Game1.perlinNoise.Noise(8 * m_Position.Y * (1 / m_Texture.Height), 8 * m_Position.Y * (1 / m_Texture.Height), .5f)) * MonteCarloCustomDistribution() * negDir;

                m_Velocity += new Vector2(x, y);

                if (m_Velocity.Length() > 0)
                {
                    m_Velocity.Normalize();
                }
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }

        private float MonteCarloCustomDistribution()
        {
            while (true)
            {
                int stepSize = Game1.cryptoRand.Next(0, 10);
                float r1 = (float)Game1.cryptoRand.Next(-stepSize, stepSize);
                float probs = r1;
                float r2 = (float)Game1.cryptoRand.Next(-stepSize, stepSize);

                if (r2 < probs)
                {
                    return r1;
                }
            }
        }
    }
}
