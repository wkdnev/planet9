using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Planet9.Source.Managers;

namespace Planet9.Source.Screens
{
    public class TitleScreen : GameScreen
    {
        private SpriteFont _font;
        private Texture2D _background;
        private Song _backgroundMusic;
        private int _selectedItem = 0;
        private string[] _menuItems = { "Play", "High Scores", "Exit" };
        private KeyboardState _prevKeyboardState;

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Arial");
            
            // Load background
            using (var stream = TitleContainer.OpenStream("Content/title_screen.png"))
            {
                _background = Texture2D.FromStream(_graphicsDevice, stream);
            }

            // Load and play music
            try
            {
                _backgroundMusic = _content.Load<Song>("game-background-title");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_backgroundMusic);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading music: " + ex.Message);
            }

            _prevKeyboardState = Keyboard.GetState();
        }

        public override void UnloadContent()
        {
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up) && !_prevKeyboardState.IsKeyDown(Keys.Up))
            {
                _selectedItem--;
                if (_selectedItem < 0) _selectedItem = _menuItems.Length - 1;
            }
            if (kstate.IsKeyDown(Keys.Down) && !_prevKeyboardState.IsKeyDown(Keys.Down))
            {
                _selectedItem++;
                if (_selectedItem >= _menuItems.Length) _selectedItem = 0;
            }

            if (kstate.IsKeyDown(Keys.Enter) && !_prevKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (_selectedItem == 0)
                {
                    LevelManager.Instance.Reset();
                    GameManager.Instance.Reset();
                    ScreenManager.Instance.LoadScreen(new GameplayScreen());
                }
                else if (_selectedItem == 1)
                {
                    ScreenManager.Instance.LoadScreen(new HighScoresScreen());
                }
                else if (_selectedItem == 2)
                {
                    System.Environment.Exit(0);
                }
            }
            _prevKeyboardState = kstate;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            if (_background != null)
            {
                spriteBatch.Draw(_background, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            }

            Vector2 center = new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);

            for (int i = 0; i < _menuItems.Length; i++)
            {
                Color color = (i == _selectedItem) ? Color.Yellow : Color.White;
                Vector2 size = _font.MeasureString(_menuItems[i]);
                spriteBatch.DrawString(_font, _menuItems[i], center - size / 2 + new Vector2(0, i * 40), color);
            }

            // Draw instructions
            string instructions = "Press Ctrl+F to Toggle Fullscreen";
            Vector2 instrSize = _font.MeasureString(instructions);
            spriteBatch.DrawString(_font, instructions, new Vector2(center.X - instrSize.X / 2, _graphicsDevice.Viewport.Height - 50), Color.Gray);
        }
    }
}