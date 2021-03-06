﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckEngine.Interfaces;
using Microsoft.Xna.Framework;
using Jitter.Dynamics;
using Jitter.LinearMath;
using DuckEngine.Input;
using DuckEngine.Helpers;

namespace DuckEngine.Managers
{
    public class MouseEventManager
    {
        private Engine Engine;
        private RigidBody mouseOver3D;
        public delegate void WhileMouseOverHandler(GameTime gameTime, InputManager input,
            RigidBody hitBody, Ray hitNormalRay, float hitFraction);

        /// <summary>
        /// method to be called while the mouse is hovering over
        /// a body.
        /// </summary>
        public WhileMouseOverHandler WhileMouseOver;

        public MouseEventManager(Engine _engine)
        {
            Engine = _engine;
            WhileMouseOver = DefaultWhileMouseOver;
        }

        public void DefaultWhileMouseOver(GameTime gameTime, InputManager input,
            RigidBody hitBody, Ray hitNormalRay, float hitFraction)
        {
            if (hitBody != mouseOver3D)
            {
                if (hitBody.Tag is IMouseEvent3D)
                {
                    ((IMouseEvent3D)hitBody.Tag).OnMouseOver();
                }
                if (mouseOver3D != null && mouseOver3D.Tag is IMouseEvent3D)
                {
                    ((IMouseEvent3D)mouseOver3D.Tag).OnMouseOut();
                }
            }
        }

        internal void ExecuteMouseEvents(GameTime gameTime, InputManager input)
        {
            //TODO
            //don't do anything if mouse is outside window bounds //Björn
            //TODO
            //check if the mouse is over any 2D (HUD) elements first
            d3(gameTime, input);
        }
        
        private void d3(GameTime gameTime, InputManager input)
        {
            Ray mouseRay = input.MouseRay;
            if (mouseRay != null) //TODO: is this check really needed? //Björn
            {
                JVector rayOrigin = mouseRay.Position.ToJitterVector();
                JVector rayDirection = mouseRay.Direction.ToJitterVector();
                RigidBody hitBody;
                JVector hitNormal;
                float hitFraction;

                bool result = Engine.Physics.CollisionSystem.Raycast(rayOrigin, rayDirection,
                    null, out hitBody, out hitNormal, out hitFraction);
                if (result && WhileMouseOver != null)
                {
                    Ray hitNormalRay;
                    hitNormalRay.Direction = hitNormal.ToXNAVector();
                    hitNormalRay.Position = mouseRay.Position + mouseRay.Direction * hitFraction;
                    WhileMouseOver(gameTime, input, hitBody, hitNormalRay, hitFraction);
                }
                mouseOver3D = hitBody;
            }
        }
    }
}
