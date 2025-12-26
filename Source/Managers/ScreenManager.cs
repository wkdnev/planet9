using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Planet9.Source.Screens;

namespace Planet9.Source.Managers
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        public static ScreenManager Instance => _instance ??= new ScreenManager();

        private GameScreen _currentScreen;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
        }

        public void LoadScreen(GameScreen screen)
        {
            if (_currentScreen != null)
            {
                _currentScreen.UnloadContent();
            }

            _currentScreen = screen;
            _currentScreen.Initialize(_content, _graphicsDevice);
            _currentScreen.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(spriteBatch);
        }
    }
}