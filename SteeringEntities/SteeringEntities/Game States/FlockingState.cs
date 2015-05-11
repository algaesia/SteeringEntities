using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SteeringEntities
{
    public class FlockingState : GameState
    {
        List<Flock> flockingEnemies = new List<Flock>();
        Player testPlayer;

        #region GUI variables
        Ruminate.GUI.Framework.Gui flockingGUI;

        //if dist less than this value, then entities will move in the same direction as each other
        Ruminate.GUI.Content.Slider alignmentDistanceSlider;
        Ruminate.GUI.Content.Label alignmentDistanceSliderValue;
        Ruminate.GUI.Content.Label alignmentDistanceSliderName;

        //if dist less than this value, then entities will move towards each other
        Ruminate.GUI.Content.Slider cohesionDistanceSlider;
        Ruminate.GUI.Content.Label cohesionDistanceSliderValue;
        Ruminate.GUI.Content.Label cohesionDistanceSliderName;

        //if dist less than this value, then entities will move away from each other
        Ruminate.GUI.Content.Slider separationDistanceSlider;
        Ruminate.GUI.Content.Label separationDistanceSliderValue;
        Ruminate.GUI.Content.Label separationDistanceSliderName;

        //determines how prominent movement towards each other is
        Ruminate.GUI.Content.Slider cohesionWeightSlider;
        Ruminate.GUI.Content.Label cohesionWeightValue;
        Ruminate.GUI.Content.Label cohesionWeightName;

        //determines how prominent movement away from each other is
        Ruminate.GUI.Content.Slider separationWeightSlider;
        Ruminate.GUI.Content.Label separationWeightValue;
        Ruminate.GUI.Content.Label separationWeightName;

        //determines how prominent movement in the same direction is
        Ruminate.GUI.Content.Slider alignmentWeightSlider;
        Ruminate.GUI.Content.Label alignmentWeightValue;
        Ruminate.GUI.Content.Label alignmentWeightName;

        //determines how prominent movement towards the centre is
        Ruminate.GUI.Content.Slider moveToCentreSlider;
        Ruminate.GUI.Content.Label moveToCentreValue;
        Ruminate.GUI.Content.Label moveToCentreName;
        #endregion

        Vector2 arbCentre = Vector2.Zero;

        public FlockingState()
            : base("FlockingState")
        {
            for (int i = 0; i < 400; ++i)
            {
                flockingEnemies.Add(new Flock(blankSquare, new Vector2(Game1.cryptoRand.Next(0, Game1.g_iScreenWidth), Game1.cryptoRand.Next(0, Game1.g_iScreenHeight)), flockingEnemies));
            }

            #region GUI setup
            Ruminate.GUI.Framework.Skin skin = new Ruminate.GUI.Framework.Skin(m_Content.Load<Texture2D>(@"GreySkin\ImageMap"), File.OpenText(@".\Content\GreySkin\Map.txt").ReadToEnd());
            Ruminate.GUI.Framework.Text text = new Ruminate.GUI.Framework.Text(m_Content.Load<SpriteFont>("spriteFont"), Color.White);
            IEnumerable<Tuple<string, Ruminate.GUI.Framework.Skin>> testSkins = new[] { new Tuple<string, Ruminate.GUI.Framework.Skin>("skin", skin) };
            IEnumerable<Tuple<string, Ruminate.GUI.Framework.Text>> testTexts = new[] { new Tuple<string, Ruminate.GUI.Framework.Text>("text", text) };

            flockingGUI = new Ruminate.GUI.Framework.Gui(GameStateManager.Instance.CurrentGame, skin, text, testSkins, testTexts)
            {
                Widgets = new Ruminate.GUI.Framework.Widget[]
                {
                    alignmentDistanceSlider = new Ruminate.GUI.Content.Slider(400, 20, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            //value is between 0 - 1, use it to multiply by max, to get current value
                            alignmentDistanceSliderValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        alignmentDistanceSliderValue = new Ruminate.GUI.Content.Label(400, 10, "0"),
                        alignmentDistanceSliderName = new Ruminate.GUI.Content.Label(400, 0, "Alignment dist multiplier"),
                    cohesionDistanceSlider = new Ruminate.GUI.Content.Slider(400, 80, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            cohesionDistanceSliderValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        cohesionDistanceSliderValue = new Ruminate.GUI.Content.Label(400, 70, "0"),
                        cohesionDistanceSliderName = new Ruminate.GUI.Content.Label(400, 60, "Cohesion dist multiplier"),
                    separationDistanceSlider = new Ruminate.GUI.Content.Slider(400, 130, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            separationDistanceSliderValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        separationDistanceSliderValue = new Ruminate.GUI.Content.Label(400, 120, "0"),
                        separationDistanceSliderName = new Ruminate.GUI.Content.Label(400, 110, "Separation dist multiplier"),

                    alignmentWeightSlider = new Ruminate.GUI.Content.Slider(700, 20, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            alignmentWeightValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        alignmentWeightValue = new Ruminate.GUI.Content.Label(700, 10, "0"),
                        alignmentWeightName = new Ruminate.GUI.Content.Label(700, 0, "Alignment weight"),
                    cohesionWeightSlider = new Ruminate.GUI.Content.Slider(700, 80, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            cohesionWeightValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        cohesionWeightValue = new Ruminate.GUI.Content.Label(700, 70, "0"),
                        cohesionWeightName = new Ruminate.GUI.Content.Label(700, 60, "Cohesion weight"),
                    separationWeightSlider = new Ruminate.GUI.Content.Slider(700, 120, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            separationWeightValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        separationWeightValue = new Ruminate.GUI.Content.Label(700, 120, "0"),
                        separationWeightName = new Ruminate.GUI.Content.Label(700, 110, "Separation weight"),
                    
                    moveToCentreSlider = new Ruminate.GUI.Content.Slider(1000, 20, 200, delegate(Ruminate.GUI.Framework.Widget slider)
                        {
                            moveToCentreValue.Value = (((Ruminate.GUI.Content.Slider)slider).Value).ToString();
                        }),
                        moveToCentreValue = new Ruminate.GUI.Content.Label(1000, 10, "0"),
                        moveToCentreName = new Ruminate.GUI.Content.Label(1000, 0, "Move towards centre weight"),

                    new Ruminate.GUI.Content.Button(200, 20, 150, "Save settings", delegate(Ruminate.GUI.Framework.Widget button)
                        {
                            SaveSettings();
                        }),

                    new Ruminate.GUI.Content.Button(200, 60, 150, "Load settings", delegate(Ruminate.GUI.Framework.Widget button)
                        {
                            LoadSettings();
                        }),

                    new Ruminate.GUI.Content.Button(10, 20, 150, "Add more boids", delegate(Ruminate.GUI.Framework.Widget button)
                        {
                            flockingEnemies.Add(new Flock(blankSquare, new Vector2(Game1.cryptoRand.Next(0, Game1.g_iScreenWidth), Game1.cryptoRand.Next(0, Game1.g_iScreenHeight)), flockingEnemies));
                        }),

                    new Ruminate.GUI.Content.Button(10, 60, 150, "Apply settings", delegate(Ruminate.GUI.Framework.Widget button)
                        {
                            foreach (Flock n in flockingEnemies)
                            {
                                n.AlignmentDist = float.Parse(alignmentDistanceSliderValue.Value) * n.MaxAlignment;
                                n.CohesionDist = float.Parse(cohesionDistanceSliderValue.Value) * n.MaxCohesion;
                                n.SeparationDist = float.Parse(separationDistanceSliderValue.Value) * n.MaxSeparation;

                                n.AlignmentWeight = float.Parse(alignmentWeightValue.Value) * n.MaxAlignmentWeight;
                                n.CohesionWeight = float.Parse(cohesionWeightValue.Value) * n.MaxCohesionWeight;
                                n.SeparationWeight = float.Parse(separationWeightValue.Value) * n.MaxSeparationWeight;

                                n.MoveToCentreWeight = float.Parse(moveToCentreValue.Value) * n.MaxMoveToCentreWeight;
                            }
                        }),
                }
            };
            #endregion

            if (File.Exists(@".\flockingSettings.bin"))
            {
                LoadSettings();
            }
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                m_BackgroundColour = Color.SlateGray;

                testPlayer = new Player(blankSquare, new Vector2(Game1.cryptoRand.Next(0, Game1.g_iScreenWidth), Game1.cryptoRand.Next(0, Game1.g_iScreenHeight)), new Vector2(32,32));

                stateCamera.CentreOnPos(Vector2.Zero);
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            testPlayer.Update(gt);

            stateCamera.CentreOnPos(testPlayer.Position);

            foreach (Flock n in flockingEnemies)
            {
                arbCentre = testPlayer.Position;//(testPlayer.Position + new Vector2(Mouse.GetState().X, Mouse.GetState().Y)) * 0.5f;
                n.arbCentrePoint = arbCentre;
                n.Update(gt);
            }

            if (Game1.IsKeyDown(Keys.R))
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.ResetPosition();
                }
            }

            //only updates if the button was pressed down on the last frame, so sets value from last frame
            if (alignmentDistanceSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.AlignmentDist = float.Parse(alignmentDistanceSliderValue.Value) * n.MaxAlignment;
                }
            }

            if (cohesionDistanceSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.CohesionDist = float.Parse(cohesionDistanceSliderValue.Value) * n.MaxCohesion;
                }
            }

            if (separationDistanceSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.SeparationDist = float.Parse(separationDistanceSliderValue.Value) * n.MaxSeparation;
                }
            }

            if (alignmentWeightSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.AlignmentWeight = float.Parse(alignmentWeightValue.Value) * n.MaxAlignmentWeight;
                }
            }

            if (cohesionWeightSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.CohesionWeight = float.Parse(cohesionWeightValue.Value) * n.MaxCohesionWeight;
                }
            }

            if (separationWeightSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.SeparationWeight = float.Parse(separationWeightValue.Value) * n.MaxSeparationWeight;
                }
            }

            if (moveToCentreSlider.IsPressed)
            {
                foreach (Flock n in flockingEnemies)
                {
                    n.MoveToCentreWeight = float.Parse(moveToCentreValue.Value) * n.MaxMoveToCentreWeight;
                }
            }

            flockingGUI.Update(gt);

            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();

            testPlayer.Draw(Game1.sb, Game1.sf);

            foreach (Flock n in flockingEnemies)
            {
                n.Draw(Game1.sb, Game1.sf);
            }

            flockingGUI.Draw();

            Game1.sb.DrawString(Game1.sf, "Flock centre", arbCentre, Color.Black);
        }

        private void LoadSettings()
        {
            if (File.Exists(@".\flockingSettings.bin"))
            {
                using (BinaryReader n = new BinaryReader(File.Open(@".\flockingSettings.bin", FileMode.Open)))
                {
                    int pos = 0;
                    int length = (int)n.BaseStream.Length;
                    while (pos < length)
                    {
                        //modify the value, as well as how it is represented by the button
                        alignmentDistanceSliderValue.Value = n.ReadSingle().ToString();
                        alignmentDistanceSlider.Value = float.Parse(alignmentDistanceSliderValue.Value);

                        cohesionDistanceSliderValue.Value = n.ReadSingle().ToString();
                        cohesionDistanceSlider.Value = float.Parse(cohesionDistanceSliderValue.Value);

                        separationDistanceSliderValue.Value = n.ReadSingle().ToString();
                        separationDistanceSlider.Value = float.Parse(separationDistanceSliderValue.Value);

                        alignmentWeightValue.Value = n.ReadSingle().ToString();
                        alignmentWeightSlider.Value = float.Parse(alignmentWeightValue.Value);

                        cohesionWeightValue.Value = n.ReadSingle().ToString();
                        cohesionWeightSlider.Value = float.Parse(cohesionWeightValue.Value);

                        separationWeightValue.Value = n.ReadSingle().ToString();
                        separationWeightSlider.Value = float.Parse(separationWeightValue.Value);

                        moveToCentreValue.Value = n.ReadSingle().ToString();
                        moveToCentreSlider.Value = float.Parse(moveToCentreValue.Value);

                        pos += sizeof(float) * 7;
                    }
                }

                foreach (Flock n in flockingEnemies)
                {
                    n.ResetPosition();
                }
            }
        }

        private void SaveSettings()
        {
            using (BinaryWriter n = new BinaryWriter(File.Open(@".\flockingSettings.bin", FileMode.Create)))
            {
                n.Write(float.Parse(alignmentDistanceSliderValue.Value));
                n.Write(float.Parse(cohesionDistanceSliderValue.Value));
                n.Write(float.Parse(separationDistanceSliderValue.Value));

                n.Write(float.Parse(alignmentWeightValue.Value));
                n.Write(float.Parse(cohesionWeightValue.Value));
                n.Write(float.Parse(separationWeightValue.Value));

                n.Write(float.Parse(moveToCentreValue.Value));
            }
        }
    }
}
