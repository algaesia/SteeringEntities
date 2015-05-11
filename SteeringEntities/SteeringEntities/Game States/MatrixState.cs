using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //state for drawing a version of the matrix letter scrolling
    public class MatrixState : GameState
    {
        DrawnCharacterSpawner matrix = new DrawnCharacterSpawner();

        public MatrixState()
            : base("MatrixState") { }

        public override void Initialise()
        {
            if (!initialised)
            {
                m_BackgroundColour = Color.Black;

                stateCamera.CentreOnPos(stateCamera.ViewportCentre);
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            matrix.Update(gt);
            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();
            matrix.Draw();
        }
    }
}
