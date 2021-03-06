﻿using DuckEngine;
using Microsoft.Xna.Framework;
using DuckEngine.Helpers;

namespace DuckGame.Weapons
{
    class Ammo1 : Projectile
    {
        public Ammo1(Engine _engine, Tracker _tracker, Vector3 _position, float _damage, float _speed, Vector3 _target, float _collisionSize)
            : base(_engine, _tracker, _position, _damage, _speed, _target, _collisionSize)
        {
            target.Normalize();
            body.LinearVelocity = Conversion.ToJitterVector(target * speed);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw3D(GameTime gameTime)
        {
            Engine.Helper3D.DrawBody(Body, Color.Gray, true, true);
        }

        public override void OnHit()
        {
            Tracker.Untrack(this);
        }
    }
}
