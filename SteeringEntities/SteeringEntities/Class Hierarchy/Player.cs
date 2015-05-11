using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SteeringEntities
{
    //contains keyboard movable entity
    public class Player : MovableEntity
    {
        string currentAnimation = "Idle";
        SpriteEffects flip = SpriteEffects.None;

        public Player(Texture2D a_Texture, Vector2 a_Position, Vector2 a_Dimensions)
            : base(a_Position, a_Dimensions, a_Texture, Color.White, 50, 100, .7f)
        {
            Animation idle;

            idle.startFrame = new Rectangle(0, 0, (int)(m_Dimensions.X), (int)m_Dimensions.Y);
            idle.endFrame = new Rectangle(0, 0, (int)(m_Dimensions.X), (int)m_Dimensions.Y);
            idle.FPS = 0;

            Animation walkLeft;

            walkLeft.startFrame = new Rectangle((int)(6 * m_Dimensions.X), 0, (int)(m_Dimensions.X), (int)m_Dimensions.Y);
            walkLeft.endFrame = new Rectangle((int)(8 * m_Dimensions.X), 0, (int)(m_Dimensions.X), (int)m_Dimensions.Y);
            walkLeft.FPS = 3;

            Animation death;

            death.startFrame = new Rectangle((int)(3 * m_Dimensions.X), (int)(2 * m_Dimensions.Y), (int)(m_Dimensions.X), (int)(m_Dimensions.Y));
            death.endFrame = new Rectangle((int)(3 * m_Dimensions.X), (int)(2 * m_Dimensions.Y), (int)(m_Dimensions.X), (int)(m_Dimensions.Y));
            death.FPS = 0;

            Animation shoot;
            shoot.startFrame = new Rectangle((int)(4 * m_Dimensions.X), (int)(2 * m_Dimensions.Y), (int)(m_Dimensions.X), (int)(m_Dimensions.Y));
            shoot.endFrame = new Rectangle((int)(4 * m_Dimensions.X), (int)(2 * m_Dimensions.Y), (int)(m_Dimensions.X), (int)(m_Dimensions.Y));
            shoot.FPS = 0;

            m_Animations.Add("Idle", idle);
            m_Animations.Add("WalkLeft", walkLeft);
            m_Animations.Add("Death", death);
            m_Animations.Add("Shoot", shoot);
        }

        public override void Update(GameTime gt)
        {
            //mouse pos - viewport centre = current target with player pos (and current camera pos) as origin
            CurrentTarget = new Vector2(Mouse.GetState().X - GameStateManager.Instance.CurrentState.StateCamera.ViewportCentre.X, Mouse.GetState().Y - GameStateManager.Instance.CurrentState.StateCamera.ViewportCentre.Y);
            //CurrentTarget = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            //reset velo if it is too small
            if (m_Velocity.Length() < 0.01f)
            {
                m_Velocity = Vector2.Zero;
            }

            //process the current command, using the input handler
            //if there is nothing to process, the current command is null
            List<Command> actions = InputHandler.Instance.HandlePlayerInput();

            //no actions to process, so start decaying the velocity
            if (actions.Count == 0)
            {
                m_Velocity *= m_VeloDecayRate;
                if (m_Velocity.Length() == 0)
                {
                    currentAnimation = "Idle";
                }
            }
            else
            {
                foreach (Command n in actions)
                {
                    n.Execute(this);
                }

                if (m_Velocity.X < 0)
                {
                    currentAnimation = "WalkLeft";
                    flip = SpriteEffects.None;
                }
                else if (m_Velocity.X > 0)
                {
                    currentAnimation = "WalkLeft";
                    flip = SpriteEffects.FlipHorizontally;
                }
            }

            //Cap velo
            if (m_Velocity.Length() != 0)
            {
                //get current length
                float mag = m_Velocity.Length();

                //normalise and extend based on which is smaller - current length of velo or cut off value
                m_Velocity = Vector2.Normalize(m_Velocity) * Math.Min(mag, MaxVelocity);
            }

            SetAnimation(m_Animations[currentAnimation]);

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            sb.Draw(m_Texture, GetRect, m_FrameCurrent, m_Colour, m_Rotation, new Vector2(m_Texture.Width * 0.5f, m_Texture.Height * 0.5f), flip, 0);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        public override void Shoot()
        {
            ProjectileManager.Instance.Shoot(m_Position - new Vector2(GetRect.Width * 4.5f, GetRect.Height * 2.1f), CurrentTarget + m_Position, true);
            currentAnimation = "Shoot";
        }
    }
}
