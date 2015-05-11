using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //http://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
    //can be manipulated with the keyboard
    //pan and zoom as well. remain
    //centred on the player
    public class Camera
    {
        public Camera(float a_CameraMovementSpeed)
        {
            Zoom = 1;

            CameraMovementSpeed = a_CameraMovementSpeed;

            //viewport is default to width and height of screen
            ViewportWidth = Game1.g_iScreenWidth;
            ViewportHeight = Game1.g_iScreenHeight;
        }

        public float CameraMovementSpeed { get; private set; }
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Vector2 ViewportCentre
        {
            //the camera now represents what is on the screen
            //this is determined by the viewport
            get { return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f); }
        }

        //Used to offset everything that is drawn on the screen.
        public Matrix TranslationMatrix
        {
            get
            {
                //return Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                //       Matrix.CreateRotationZ(Rotation) *  
                //       Matrix.CreateTranslation(new Vector3(ViewportCentre, 0)) *
                //       Matrix.CreateTranslation(-(int)(Position.X), -(int)(Position.Y), 0);
                return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                       Matrix.CreateTranslation(new Vector3(ViewportCentre, 0));
            }
        }

        //No checks for maximum zoom value
        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }

            if (Zoom > 25.0f)
            {
                Zoom = 25.0f;
            }
        }

        //for keyboard controls/movement
        private void MoveCameraBy(Vector2 positionToMoveTo)
        {
            Position += positionToMoveTo;
        }

        //for following a particular entity
        public void CentreOnPos(Vector2 a_Position)
        {
            Position = a_Position;
        }

        public Rectangle ViewportBoundary
        {
            get
            {
                Vector2 viewportCorner = ScreenToWorld(Vector2.Zero);
                Vector2 viewportBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));
                return new Rectangle((int)viewportCorner.X, (int)viewportCorner.Y, (int)(viewportBottomCorner.X - viewportCorner.X), (int)(viewportBottomCorner.Y - viewportCorner.Y));
            }
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

        public void HandleInput()
        {
            Vector2 cameraMovement = Vector2.Zero;

            if (Game1.currentKeyboardState.IsKeyDown(Keys.Up))
            {
                cameraMovement.Y = -1;
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.Down))
            {
                cameraMovement.Y = 1;
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                cameraMovement.X = -1;
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                cameraMovement.X = 1;
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.NumPad2))
            {
                AdjustZoom(-.01f);
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.NumPad8))
            {
                AdjustZoom(.01f);
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.NumPad4))
            {
                Rotation += 0.01f;
            }

            if (Game1.currentKeyboardState.IsKeyDown(Keys.NumPad6))
            {
                Rotation -= 0.01f;
            }

            cameraMovement *= CameraMovementSpeed;

            MoveCameraBy(cameraMovement);
        }
    }
}
