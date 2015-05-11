using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class AudioState : GameState
    {
        public AudioState()
            : base("AudioState")
        {
            
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                EffectManager.Instance.AddEffect("Explosion");
                EffectManager.Instance.AddEffect("Sidewinder");
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            if (Game1.IsKeyDown(Keys.U))
            {
                EffectManager.Instance.PlayEffect("Sidewinder");
            }

            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
