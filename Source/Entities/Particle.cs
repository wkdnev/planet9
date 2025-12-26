using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet9.Source.Entities
{
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;
        public float LifeTime;
        public bool IsActive => LifeTime > 0;

        public void Update(float dt)
        {
            Position += Velocity * dt;
            LifeTime -= dt;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Simple 1x1 pixel texture or passed texture
            spriteBatch.Draw(texture, Position, Color * LifeTime);
        }
    }
}