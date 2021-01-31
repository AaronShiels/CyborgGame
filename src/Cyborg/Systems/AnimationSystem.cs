using System.Collections.Generic;
using System.Linq;
using Cyborg.Components;
using Cyborg.ContentPipeline;
using Cyborg.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyborg.Systems
{
    public class AnimationSystem : IUpdateSystem
    {
        private const int _frameRate = 4;
        private readonly ContentManager _contentManager;

        public AnimationSystem(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Update(IEnumerable<IEntity> entities, GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entities.OfType<IAnimated>())
            {
                entity.AnimationElapsed += elapsed;

                var animationSet = _contentManager.Load<AnimationSet>(entity.AnimationSet);
                var spriteSheet = _contentManager.Load<Texture2D>(entity.SpriteSheet);

                var animation = animationSet.Animations[entity.Animation];
                var currentAnimationFrameIndex = (int)(entity.AnimationElapsed * _frameRate) % animation.Length;
                var spriteFrameIndex = animation[currentAnimationFrameIndex];
                var spriteFrameOffsetX = spriteFrameIndex * animationSet.FrameWidth % spriteSheet.Width;
                var spriteFrameOffsetY = spriteFrameIndex * animationSet.FrameWidth / spriteSheet.Width * animationSet.FrameHeight;
                var rectangle = new Rectangle(spriteFrameOffsetX, spriteFrameOffsetY, animationSet.FrameWidth, animationSet.FrameHeight);

                entity.SpriteFrame = rectangle;
            }
        }
    }
}