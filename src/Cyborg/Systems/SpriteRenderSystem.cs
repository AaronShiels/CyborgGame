using System;
using System.Collections.Generic;
using System.Linq;
using Cyborg.Components;
using Cyborg.Core;
using Cyborg.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyborg.Systems
{
    public class SpriteRenderSystem : IDrawSystem
    {
        private readonly IReadOnlyCollection<IEntity> _entities;
        private readonly IGameState _gameState;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Matrix _globalTransform;
        private readonly Texture2D _debugPixel;

        public SpriteRenderSystem(IReadOnlyCollection<IEntity> entities, IGameState gameState, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _entities = entities;
            _gameState = gameState;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;

            _globalTransform = ComputeScalingTransform(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, Constants.BaseWidth, Constants.BaseHeight);
            _debugPixel = new Texture2D(graphicsDevice, 1, 1);
            _debugPixel.SetData(new Color[] { Color.Blue });
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(new Color(57, 49, 75));

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _globalTransform);

            var cameraFrame = GetCameraFrame();
            RenderSprites(cameraFrame);
            RenderDebug(cameraFrame);

            _spriteBatch.End();
        }

        private Rectangle GetCameraFrame()
        {
            var cameraEntity = _entities.OfType<Camera>().Single();
            var position = Vector2.Round(cameraEntity.Position).ToPoint();

            return new Rectangle(position.X - Constants.BaseWidth / 2, position.Y - Constants.BaseHeight / 2, Constants.BaseWidth, Constants.BaseHeight);
        }

        private void RenderSprites(Rectangle cameraFrame)
        {
            foreach (var entity in _entities.OfType<ISprite>().OrderBy(e => e.Sprite.Order))
            {
                var position = Vector2.Round(entity.Position).ToPoint();
                var frame = new Rectangle(position.X + entity.Sprite.Offset.X, position.Y + entity.Sprite.Offset.Y, entity.Sprite.Frame.Width, entity.Sprite.Frame.Height);
                if (frame.Right < cameraFrame.Left || frame.Left > cameraFrame.Right || frame.Bottom < cameraFrame.Top || frame.Top > cameraFrame.Bottom)
                    continue;

                _spriteBatch.Draw(entity.Sprite.Texture, (frame.Location - cameraFrame.Location).ToVector2(), entity.Sprite.Frame, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        private void RenderDebug(Rectangle cameraFrame)
        {
            if (!_gameState.Debug)
                return;

            foreach (var entity in _entities.OfType<IBody>())
            {
                var entityFrame = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.Size.X, entity.Size.Y);
                if (entityFrame.Right < cameraFrame.Left || entityFrame.Left > cameraFrame.Right || entityFrame.Bottom < cameraFrame.Top || entityFrame.Top > cameraFrame.Bottom)
                    continue;

                if (entity.Edges.HasFlag(Edge.Right))
                    _spriteBatch.Draw(_debugPixel, new Rectangle(entityFrame.Right, entityFrame.Top, 1, entityFrame.Height), Color.White);

                if (entity.Edges.HasFlag(Edge.Left))
                    _spriteBatch.Draw(_debugPixel, new Rectangle(entityFrame.Left, entityFrame.Top, 1, entityFrame.Height), Color.White);

                if (entity.Edges.HasFlag(Edge.Bottom))
                    _spriteBatch.Draw(_debugPixel, new Rectangle(entityFrame.Left, entityFrame.Bottom, entityFrame.Width, 1), Color.White);

                if (entity.Edges.HasFlag(Edge.Top))
                    _spriteBatch.Draw(_debugPixel, new Rectangle(entityFrame.Left, entityFrame.Top, entityFrame.Width, 1), Color.White);
            }
        }

        private static Matrix ComputeScalingTransform(float screenX, float screenY, float baseX, float baseY) => Matrix.CreateScale(new Vector3(screenX / baseX, screenY / baseY, 1));
    }
}