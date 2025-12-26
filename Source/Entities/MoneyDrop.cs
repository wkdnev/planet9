using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet9.Source.Entities
{
    public class MoneyDrop
    {
        public Vector2 Position;
        public Texture2D Texture;
        public int Value;
        public bool IsActive;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public MoneyDrop(Texture2D texture, Vector2 position, int value)
        {
            Texture = texture;
            Position = position;
            Value = value;
            IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y += 150f * dt; // Fall down
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(Texture, Position, Color.Gold);
            }
        }
    }
}