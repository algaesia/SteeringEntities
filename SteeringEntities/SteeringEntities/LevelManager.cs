using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //Loads in level from bitmap
    public class LevelManager
    {
        List<Entity> levelEntities = new List<Entity>();

        int m_LevelWidth, m_LevelHeight;

        Texture2D m_LevelTexture;
        Player playerRef;

        public LevelManager(Texture2D a_LevelTexture)
        {
            m_LevelTexture = a_LevelTexture;
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            m_LevelWidth = m_LevelTexture.Width;
            m_LevelHeight = m_LevelTexture.Height;

            Color[] colours = new Color[m_LevelWidth * m_LevelHeight];

            m_LevelTexture.GetData(colours);

            Color[,] colourData = new Color[m_LevelWidth, m_LevelHeight];

            for (int i = 0; i < m_LevelWidth; ++i)
            {
                for (int j = 0; j < m_LevelHeight; ++j)
                {
                    colourData[i, j] = colours[i + j * m_LevelWidth];
                }
            }

            Color currentColour = Color.White;

            for (int i = 0; i < m_LevelWidth; ++i)
            {
                for (int j = 0; j < m_LevelHeight; ++j)
                {
                    currentColour = colourData[i, j];

                    if (currentColour.PackedValue == Color.Black.PackedValue)
                    {
                        levelEntities.Add(new Obstacle(new Vector2(i * 32, j * 32), new Vector2(32, 32), Game1.whiteSquare, Color.Black));
                    }
                    else if (currentColour.PackedValue == Color.White.PackedValue)
                    {
                        
                    }
                    else if (currentColour.PackedValue == Color.Red.PackedValue)
                    {
                        levelEntities.Add(new RandomWalker(new Vector2(i * 32, j * 32), Game1.whiteSquare));
                    }
                    else if (currentColour.PackedValue == Color.Blue.PackedValue)
                    {
                        
                    }
                    else if (currentColour.PackedValue == Color.Yellow.PackedValue)
                    {
                        playerRef = new Player(Game1.whiteSquare, new Vector2(i * 32, j * 32), new Vector2(32, 32));
                        levelEntities.Add(playerRef);
                    }
                }
            }
        }

        public void Update(GameTime gt)
        {
            foreach (Entity n in levelEntities)
            {
                n.Update(gt);
            }
        }

        public void Draw()
        {
            foreach (Entity n in levelEntities)
            {
                n.Draw(Game1.sb, Game1.sf);
            }
        }

        public Player PlayerRef
        {
            get { return playerRef; }
        }
    }
}
