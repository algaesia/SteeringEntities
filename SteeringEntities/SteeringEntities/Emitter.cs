using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Particle : AnimatedEntity
    {
        Vector2 m_Velocity;
        Vector2 m_Acceleration;

        public Particle(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour)
            : base(a_Position, a_Dimensions, a_Texture, a_Colour)
        {
            IsAlive = false;

            Animation one;

            //one.startFrame = new Rectangle(-32, 0, 32, 32);
            //one.endFrame = new Rectangle(4 * 32, 0, 32, 32);
            //one.FPS = (float)(Game1.cryptoRand.NextDouble() * 40);

            one.startFrame = new Rectangle(-210, 0, 210, 210);
            one.endFrame = new Rectangle(4 * 210, 0, 210, 210);
            one.FPS = (float)(Game1.cryptoRand.NextDouble() * 40);

            m_Animations.Add("One", one);

            SetAnimation(m_Animations["One"]);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            a_SB.Draw(m_Texture, GetRect, m_FrameCurrent, m_Colour * Alpha, m_Rotation, new Vector2(m_Texture.Width * 0.5f, m_Texture.Height * 0.5f), SpriteEffects.None, 0);
        }

        public float Alpha
        {
            get;
            set;
        }

        public float Life
        {
            get;
            set;
        }

        public class Emitter : DrawableEntity
        {
            const int MAX_PARTICLES = 1000;

            List<Particle> m_Particles = new List<Particle>();
            float m_ElapsedTime = 0;
            float m_EmissionRate;

            float m_MinLife, m_MaxLife;
            float m_MinSize, m_MaxSize;
            float m_MinVelocity, m_MaxVelocity;
            float m_MinDistFromParent, m_MaxDistFromParent;
            float m_Gravity;
            float m_Wind;

            //single sprite sheet
            public Emitter(Vector2 a_Position, Texture2D a_Texture, Color a_Colour)
                : base(a_Position, Vector2.Zero, null, a_Colour)
            {
                m_Texture = a_Texture;

                m_EmissionRate = 250;
                m_MinLife = 0.5f;
                m_MaxLife = 4;
                m_MinSize = 8;
                m_MaxSize = 32;
                m_MinVelocity = -100;
                m_MaxVelocity = 100;
                m_Gravity = 9.8f;
                m_Wind = -2;
                m_MinDistFromParent = 1;
                m_MaxDistFromParent = 10;

                for (int i = 0; i < MAX_PARTICLES; ++i)
                {
                    Particle t = new Particle(m_Position + new Vector2((float)(Game1.cryptoRand.NextDouble()), (float)(Game1.cryptoRand.NextDouble())), new Vector2(m_MinSize + (float)Game1.cryptoRand.NextDouble() * (m_MaxSize - m_MinSize), m_MinSize + (float)Game1.cryptoRand.NextDouble() * (m_MaxSize - m_MinSize)), m_Texture, Color.White);
                    t.Life = m_MinLife + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxLife - m_MinLife);
                    t.m_Acceleration = new Vector2(m_Wind, -m_Gravity);
                    t.m_Velocity = new Vector2(m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity), m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity));

                    m_Particles.Add(t);
                }
            }

            //multiple sprite sheets
            public Emitter(Vector2 a_Position, Vector2 a_Dimensions, Texture2D [] a_Texture, Color a_Colour)
                : base(a_Position, a_Dimensions, null, a_Colour)
            {
                m_EmissionRate = 1000;
                m_MinLife = 0.5f;
                m_MaxLife = 8;
                m_MinSize = 8;
                m_MaxSize = 32;
                m_MinVelocity = -50;
                m_MaxVelocity = 50;
                m_Gravity = 1;
                m_Wind = 0.5f;
                m_MinDistFromParent = 10;
                m_MaxDistFromParent = 20;

                for (int i = 0; i < MAX_PARTICLES; ++i)
                {
                    int rand = (int)(Game1.cryptoRand.NextDouble() * a_Texture.Length);
                    m_Texture = a_Texture[rand];

                    Particle t = new Particle(m_Position + new Vector2((float)(Game1.cryptoRand.NextDouble()), (float)(Game1.cryptoRand.NextDouble())), new Vector2(m_MinSize + (float)Game1.cryptoRand.NextDouble() * (m_MaxSize - m_MinSize), m_MinSize + (float)Game1.cryptoRand.NextDouble() * (m_MaxSize - m_MinSize)), m_Texture, Color.White);
                    t.Life = m_MinLife + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxLife - m_MinLife);
                    t.m_Acceleration = new Vector2(m_Wind, -m_Gravity);
                    t.m_Velocity = new Vector2(m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity), m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity));

                    m_Particles.Add(t);
                }
            }

            public override void Update(GameTime gt)
            {
                float dt = (float)(gt.ElapsedGameTime.TotalSeconds);
                m_ElapsedTime += dt;

                m_Wind = (float)Game1.cryptoRand.NextDouble();
                m_Gravity = (float)Game1.cryptoRand.NextDouble();

                while (m_ElapsedTime > (1.0f / m_EmissionRate))
                {
                    SpawnParticle();
                    m_ElapsedTime -= (1.0f / m_EmissionRate);
                }

                for (int i = m_Particles.Count - 1; i >= 0; --i)
                {
                    Particle currentParticle = m_Particles[i];

                    currentParticle.Life -= (float)(gt.ElapsedGameTime.TotalSeconds);

                    if (currentParticle.Life <= 0)
                    {
                        currentParticle.IsAlive = false;
                    }

                    currentParticle.m_Acceleration.Y += m_Gravity * dt;
                    currentParticle.m_Acceleration.X += m_Wind * dt;

                    currentParticle.m_Velocity += currentParticle.m_Acceleration * dt;

                    currentParticle.m_Position.X += currentParticle.m_Velocity.X * dt;
                    currentParticle.m_Position.Y -= currentParticle.m_Velocity.Y * dt;

                    currentParticle.Alpha = 1 - (currentParticle.Life / m_MaxLife);

                    currentParticle.Update(gt);
                }

                base.Update(gt);
            }

            public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
            {
                foreach (Particle n in m_Particles)
                {
                    n.Draw(a_SB, a_SF);
                }
            }

            private void SpawnParticle()
            {
                foreach (Particle n in m_Particles)
                {
                    if (!n.IsAlive)
                    {
                        n.IsAlive = true;
                        n.Position = m_Position + new Vector2(m_MinDistFromParent * (float)(Game1.cryptoRand.NextDouble()) * (m_MaxDistFromParent - m_MinDistFromParent), m_MinDistFromParent * (float)(Game1.cryptoRand.NextDouble()) * (m_MaxDistFromParent - m_MinDistFromParent));
                        n.Life = m_MinLife + (m_MaxLife - m_MinLife) * (float)(Game1.cryptoRand.NextDouble());
                        n.m_Acceleration = new Vector2(m_Wind, -m_Gravity);
                        n.m_Velocity = new Vector2(m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity), m_MinVelocity + (float)(Game1.cryptoRand.NextDouble()) * (m_MaxVelocity - m_MinVelocity));
                    }
                }
            }
        }
    }
}
