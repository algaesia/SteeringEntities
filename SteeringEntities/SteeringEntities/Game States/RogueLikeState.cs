using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class RogueLikeState : GameState
    {
        List<Obstacle> obstacles = new List<Obstacle>();
        //Particle.Emitter particleEmitter;

        Player test;

        public RogueLikeState()
            : base("RogueLikeState")
        {
            m_BackgroundColour = Color.White;
            obstacles.Clear();
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                test = new Player(m_Content.Load<Texture2D>("kefka_cropped"), new Vector2(40, 50), new Vector2(48, 48));

                //particleEmitter = new Particle.Emitter(Game1.RandomPosition(), new Texture2D [] {m_Content.Load<Texture2D>("animatedColours1"), m_Content.Load<Texture2D>("animatedColours2"), m_Content.Load<Texture2D>("animatedColours3"), m_Content.Load<Texture2D>("animatedColours4"), m_Content.Load<Texture2D>("animatedColours5")}, Game1.RandomColour());
                //particleEmitter = new Particle.Emitter(Game1.RandomPosition(), m_Content.Load<Texture2D>("smoke"), Game1.RandomColour());

                int maxObstacles = 500;

                float obstacleMinWidth = 300;
                float obstacleMaxWidth = 500;

                float obstacleMinHeight = 300;
                float obstacleMaxHeight = 500;

                float minXOffset = -4000;
                float maxXOffset = 4000;

                float minYOffset = -4000;
                float maxYOffset = 4000;

                for (int i = 0; i < maxObstacles; ++i)
                {
                    Obstacle newRoom = new Obstacle(test.Position + new Vector2(minXOffset + (float)(Game1.cryptoRand.NextDouble() * (maxXOffset - minXOffset)), minYOffset + (float)(Game1.cryptoRand.NextDouble() * (maxYOffset - minYOffset))),
                                                    new Vector2(obstacleMinWidth + (float)(Game1.cryptoRand.NextDouble() * (obstacleMaxWidth - obstacleMinWidth)), obstacleMinHeight + (float)(Game1.cryptoRand.NextDouble() * (obstacleMaxHeight - obstacleMinHeight))),
                                                    blankSquare,
                                                    Game1.RandomColour());

                    bool newRoomIntersects = false;

                    for (int j = 0; j < obstacles.Count; ++j)
                    {
                        if (newRoom.GetRect.Intersects(obstacles[j].GetRect))
                        {
                            newRoomIntersects = true;
                            break;
                        }
                    }

                    if (!newRoomIntersects)
                    {
                        obstacles.Add(newRoom);
                    }
                }

                //adding passage ways
                //List<Obstacle> newRooms = new List<Obstacle>();
                //for (int i = 1; i < obstacles.Count; ++i)
                //{
                //    int prevRoomCentreX = obstacles[i - 1].GetRect.Center.X;
                //    int prevRoomCentreY = obstacles[i - 1].GetRect.Center.Y;

                //    int currRoomCentreX = obstacles[i].GetRect.Center.X;
                //    int currRoomCentreY = obstacles[i].GetRect.Center.Y;

                //    Obstacle newRoom = new Obstacle(new Vector2(prevRoomCentreX - currRoomCentreX, prevRoomCentreY - currRoomCentreY), new Vector2(32), blankSquare, Color.Black);

                //    if (currRoomCentreX < prevRoomCentreX)
                //    {
                //        Vector2 temp = new Vector2(32, Math.Abs(prevRoomCentreX - currRoomCentreX));
                //        newRoom.Dimensions = temp;
                //    }
                //    else if (currRoomCentreX > prevRoomCentreX)
                //    {
                //        Vector2 temp = new Vector2(32, Math.Abs(currRoomCentreX - prevRoomCentreX));
                //        newRoom.Dimensions = temp;
                //    }

                //    if (currRoomCentreY < prevRoomCentreY)
                //    {
                //        Vector2 temp = new Vector2(Math.Abs(prevRoomCentreY - currRoomCentreY), 32);
                //        newRoom.Dimensions = temp;
                //    }
                //    else if (currRoomCentreY < prevRoomCentreY)
                //    {
                //        Vector2 temp = new Vector2(Math.Abs(currRoomCentreY - prevRoomCentreY), 32);
                //        newRoom.Dimensions = temp;
                //    }

                //    newRooms.Add(newRoom);
                //}

                //obstacles.AddRange(newRooms);

                stateCamera.CentreOnPos(new Vector2(Game1.g_iScreenWidth * 0.5f, Game1.g_iScreenHeight * 0.5f));
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            foreach (Obstacle n in obstacles)
            {
                n.Update(gt);
            }

            ProjectileManager.Instance.Update(gt);

            stateCamera.CentreOnPos(new Vector2(test.Position.X - test.GetRect.Width, test.Position.Y - test.GetRect.Height));
            //stateCamera.HandleInput();

            test.Update(gt);

            //particleEmitter.Position = new Vector2(test.Position.X - test.GetRect.Width * 4, test.Position.Y - test.GetRect.Height * 1.2f);
            //particleEmitter.Position = stateCamera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

            //particleEmitter.Update(gt);

            base.Update(gt);
        }

        public override void Draw()
        {
            //particleEmitter.Draw(Game1.sb, Game1.sf);

            foreach (Obstacle n in obstacles)
            {
                n.Draw(Game1.sb, Game1.sf);
            }

            ProjectileManager.Instance.Draw(Game1.sb, Game1.sf);

            test.Draw(Game1.sb, Game1.sf);

            base.Draw();
        }
    }
}
