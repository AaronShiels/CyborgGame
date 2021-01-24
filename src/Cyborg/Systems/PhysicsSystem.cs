using System;
using System.Linq;
using Cyborg.Components;
using Cyborg.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyborg.Systems
{
    public class PhysicsSystem : IUpdateSystem
    {
        private const float _dragCoefficient = 0.2f;
        private const float _stoppingThreshold = 10f;

        private readonly IEntityManager _entityManager;
        private readonly GraphicsDevice _graphicsDevice;

        public PhysicsSystem(IEntityManager entityManager, GraphicsDevice graphicsDevice)
        {
            _entityManager = entityManager;
            _graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            var entities = _entityManager.Get<IMovingBody>();
            if (!entities.Any())
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entities)
            {
                if (Debug.Enabled)
                    Debug.Add(entity, "DrivingForce", $"{entity.Force.Length():F}");

                // Apply drag to force
                // Fd = kv²
                // F2 = 1 - Fd/F1
                var dragScalar = _dragCoefficient * entity.Velocity.LengthSquared();
                var dragForce = dragScalar > 0
                    ? -dragScalar * Vector2.Normalize(entity.Velocity)
                    : Vector2.Zero;

                if (Debug.Enabled)
                    Debug.Add(entity, "DragForce", $"{dragScalar:F}");

                entity.Force += dragForce;

                if (Debug.Enabled)
                    Debug.Add(entity, "Force", $"{entity.Force.Length():F}");

                // Apply force to velocity
                // F = ma, a = F/m
                var acceleration = entity.Force / entity.Mass;
                entity.Velocity += acceleration * elapsed;

                // Apply stopping thresholds
                if (entity.Velocity.Length() < _stoppingThreshold)
                    entity.Velocity = Vector2.Zero;

                if (Debug.Enabled)
                    Debug.Add(entity, "Velocity", $"{entity.Velocity.Length():F}");

                // Apply velocity
                entity.Position += entity.Velocity * elapsed;

                if (Debug.Enabled)
                    Debug.Add(entity, "Position", $"{entity.Position.X:F}, {entity.Position.Y:F}");

                // Apply collision
                var horizontalCollision = entity.Position.X - entity.Size.X / 2 < 0 || entity.Position.X + entity.Size.X / 2 > Constants.BaseWidth;
                var verticalCollision = entity.Position.Y - entity.Size.Y / 2 < 0 || entity.Position.Y + entity.Size.Y / 2 > Constants.BaseHeight;

                if (horizontalCollision || verticalCollision)
                {
                    entity.Position = new Vector2(MathHelper.Clamp(entity.Position.X, entity.Size.X / 2, Constants.BaseWidth - entity.Size.X / 2), MathHelper.Clamp(entity.Position.Y, entity.Size.Y / 2, Constants.BaseHeight - entity.Size.Y / 2));
                    entity.Velocity = new Vector2(horizontalCollision ? 0 : entity.Velocity.X, verticalCollision ? 0 : entity.Velocity.Y);
                }
            }
        }
    }
}