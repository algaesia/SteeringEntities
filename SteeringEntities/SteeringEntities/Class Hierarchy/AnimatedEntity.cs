using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class AnimatedEntity : Entity
    {
        protected struct Animation
        {
            public Rectangle startFrame;
            public Rectangle endFrame;
            public float FPS;

            public Animation(Rectangle a_Start, Rectangle a_End, float a_FPS)
            {
                startFrame = a_Start;
                endFrame = a_End;
                FPS = a_FPS;
            }
        }

        protected Dictionary<string, Animation> m_Animations = new Dictionary<string, Animation>();
        protected Texture2D m_Texture;
        protected Color m_Colour;
        protected bool m_IsPlaying;
        protected float m_FrameTimer;

        protected Animation m_AnimationCurrent;
        protected Rectangle m_FrameCurrent;

        public AnimatedEntity(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour)
            : base(a_Position, a_Dimensions)
        {
            m_Texture = a_Texture;
            m_Colour = a_Colour;

            m_IsPlaying = true;

            m_FrameTimer = 0;
        }

        public override void Update(GameTime gt)
        {
            if (m_IsPlaying)
            {
                m_FrameTimer += (float)gt.ElapsedGameTime.TotalSeconds;
                if (m_FrameTimer > 1 / m_AnimationCurrent.FPS)
                {
                    m_FrameTimer = 0;

                    if (m_FrameCurrent.X >= m_AnimationCurrent.endFrame.X)
                    {
                        m_FrameCurrent = m_AnimationCurrent.startFrame;
                    }

                    m_FrameCurrent.X += m_FrameCurrent.Width;
                }
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            if (m_Animations.Count > 0)
            {
                a_SB.Draw(m_Texture, GetRect, m_FrameCurrent, m_Colour, m_Rotation, new Vector2(m_Texture.Width * 0.5f, m_Texture.Height * 0.5f), SpriteEffects.None, 0);
            }
            else
            {
                a_SB.Draw(m_Texture, GetRect, null, m_Colour, m_Rotation, new Vector2(m_Texture.Width * 0.5f, m_Texture.Height * 0.5f), SpriteEffects.None, 0);
            }
        }

        protected void SetAnimation(Animation Anim)
        {
            // if current animation does not equal new animation
            if (m_AnimationCurrent.startFrame != Anim.startFrame ||
                    m_AnimationCurrent.endFrame != Anim.endFrame ||
                    m_AnimationCurrent.FPS != Anim.FPS)
            {   // set animation to new animation
                m_AnimationCurrent.startFrame = Anim.startFrame;
                m_AnimationCurrent.endFrame = Anim.endFrame;
                m_AnimationCurrent.FPS = Anim.FPS;
                //reset animation to start again after it has finished, so that it can loop
                m_FrameTimer = 0.0f;
                m_FrameCurrent = Anim.startFrame;
            }
        } 
    }
}
