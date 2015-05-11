using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class DrawnCharacterSpawner
    {
        List<DrawnCharacter> characters = new List<DrawnCharacter>();

        int widthMultiplier = 20;

        public DrawnCharacterSpawner()
        {
            for (int i = 0; i <= (int)(Game1.g_iScreenWidth / widthMultiplier); ++i)
            {
                for (int j = 0; j <= (int)(Game1.g_iScreenWidth / widthMultiplier); ++j)
                {
                    characters.Add(new DrawnCharacter(new Vector2(i * widthMultiplier, -10)));
                }
            }
        }

        public void Update(GameTime gt)
        {
            foreach (DrawnCharacter n in characters)
            {
                if (!n.IsAlive)
                {
                    n.Reset();
                }

                n.Update(gt);
            }
        }

        public void Draw()
        {
            foreach (DrawnCharacter n in characters)
            {
                n.Draw(Game1.sb, Game1.sf);
            }
        }
    }
}
