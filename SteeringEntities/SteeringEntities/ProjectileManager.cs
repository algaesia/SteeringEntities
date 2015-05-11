using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class ProjectileManager
    {
        private static ProjectileManager instance;

        private List<Projectile> projectiles = new List<Projectile>();
        private const int MAX_NUM_PROJECTILES = 100;

        private ProjectileManager()
        {
            for (int i = 0; i < MAX_NUM_PROJECTILES; ++i)
            {
                projectiles.Add(new Projectile(new Vector2(GameStateManager.Instance.CurrentState.StateCamera.ViewportBoundary.Width * 2, GameStateManager.Instance.CurrentState.StateCamera.ViewportBoundary.Height * 2), new Vector2(8, 8), Game1.whiteSquare, Color.Black, 10, 50, 0.9f));
            }
        }

        public static ProjectileManager Instance
        {
            get { return instance == null ? instance = new ProjectileManager() : instance; }
        }

        public void Shoot(Vector2 a_Position, Vector2 a_TargetPosition, bool isPlayerBullet)
        {
            foreach (Projectile n in projectiles)
            {
                if (!n.IsAlive)
                {
                    n.IsPlayerBullet = isPlayerBullet;
                    n.IsAlive = true;
                    n.Position = a_Position;
                    n.StartingPosition = n.Position;
                    n.Target = a_TargetPosition;
                    return;
                }
            }
        }

        public void Update(GameTime gt)
        {
            projectiles = projectiles.OrderBy(o => o.IsAlive).ToList();

            foreach (Projectile n in projectiles)
            {
                n.Update(gt);
            }
        }

        public void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            foreach (Projectile n in projectiles)
            {
                n.Draw(a_SB, a_SF);
            }
        }
    }
}
