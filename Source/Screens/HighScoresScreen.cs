using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Planet9.Source.Managers;

namespace Planet9.Source.Screens
{
    public class HighScoresScreen : GameScreen
    {
        private SpriteFont _font;

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Arial");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenManager.Instance.LoadScreen(new TitleScreen());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "HIGH SCORES", new Vector2(100, 50), Color.White);

            var scores = LeaderboardManager.Instance.HighScores;
            for (int i = 0; i < scores.Count && i < 10; i++)
            {
                spriteBatch.DrawString(_font, $"{i + 1}. {scores[i].Name} - {scores[i].Score}", new Vector2(100, 100 + i * 30), Color.White);
            }
            
            spriteBatch.DrawString(_font, "Press ESC to Return", new Vector2(100, 500), Color.Yellow);
        }
    }
}