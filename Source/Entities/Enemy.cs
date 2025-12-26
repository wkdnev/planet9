using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Planet9.Source.Entities
{
    public class Enemy
    {
        public Vector2 Position;
        public Texture2D Texture;
        public int Health;
        public float Speed;
        public int ScoreValue;
        public float FireChance;
        public bool IsActive;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        private Vector2 _targetPosition;
        private Random _random;
        private float _fireTimer;

        public Enemy(Texture2D texture, Vector2 position, int health, float speed, int scoreValue, float fireChance)
        {
            Texture = texture;
            Position = position;
            Health = health;
            Speed = speed;
            ScoreValue = scoreValue;
            FireChance = fireChance;
            IsActive = true;
            _random = new Random();
            
            // Initial target
            PickNewTarget();
        }

        private void PickNewTarget()
        {
            // Target random position in top half of screen (assuming 1280x720)
            // We can pass viewport bounds in Update if needed, but hardcoding/assuming for now or passing in ctor is easier.
            // Let's assume standard resolution or pass it.
            // Better: Pass viewport width/height to Update.
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight, List<Enemy> otherEnemies, Action<Vector2> onShoot)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movement Logic: Swarm / Hover in top two-thirds
            if (_targetPosition == Vector2.Zero || Vector2.Distance(Position, _targetPosition) < 10f)
            {
                // Pick new target in top two-thirds (0 to screenHeight * 2/3)
                // Keep some padding
                int padding = 50;
                int maxY = (int)(screenHeight * 0.66f);
                _targetPosition = new Vector2(
                    _random.Next(padding, screenWidth - padding),
                    _random.Next(padding, maxY)
                );
            }

            // Calculate Separation Force
            Vector2 separation = Vector2.Zero;
            float separationRadius = Texture.Width * 1.2f; // Radius to check for neighbors
            int neighbors = 0;

            foreach (var other in otherEnemies)
            {
                if (other == this || !other.IsActive) continue;

                float dist = Vector2.Distance(Position, other.Position);
                if (dist < separationRadius)
                {
                    Vector2 push = Position - other.Position;
                    if (push.LengthSquared() > 0)
                    {
                        push.Normalize();
                        separation += push / dist; // Stronger push if closer
                        neighbors++;
                    }
                }
            }

            // Move towards target with separation
            Vector2 moveDirection = _targetPosition - Position;
            if (moveDirection != Vector2.Zero) moveDirection.Normalize();

            if (neighbors > 0)
            {
                separation /= neighbors;
                if (separation != Vector2.Zero) separation.Normalize();
                // Blend movement: Target + Separation
                moveDirection = moveDirection + (separation * 2.0f); // Give separation high priority
                if (moveDirection != Vector2.Zero) moveDirection.Normalize();
            }

            Position += moveDirection * Speed * dt;

            // Firing Logic
            _fireTimer -= dt;
            if (_fireTimer <= 0)
            {
                _fireTimer = 1.0f; // Check every second
                if (_random.NextDouble() < FireChance)
                {
                    onShoot?.Invoke(new Vector2(Position.X + Texture.Width / 2, Position.Y + Texture.Height));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(Texture, Position, Color.Red);
            }
        }
    }
}