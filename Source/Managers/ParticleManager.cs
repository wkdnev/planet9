using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Planet9.Source.Entities;
using System.Collections.Generic;

namespace Planet9.Source.Managers
{
    public class ParticleManager
    {
        private static ParticleManager _instance;
        public static ParticleManager Instance => _instance ??= new ParticleManager();

        private List<Particle> _particles;
        private Texture2D _pixelTexture;
        private System.Random _random;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            _particles = new List<Particle>();
            _pixelTexture = new Texture2D(graphicsDevice, 2, 2);
            _pixelTexture.SetData(new Color[] { Color.White, Color.White, Color.White, Color.White });
            _random = new System.Random();
        }

        public void SpawnExplosion(Vector2 position, Color color, int count = 20)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)(_random.NextDouble() * System.Math.PI * 2);
                float speed = _random.Next(50, 200);
                Vector2 velocity = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * speed;

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Color = color,
                    LifeTime = 0.5f + (float)_random.NextDouble() * 0.5f
                });
            }
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Update(dt);
                if (!_particles[i].IsActive)
                {
                    _particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
            {
                p.Draw(spriteBatch, _pixelTexture);
            }
        }
    }
}