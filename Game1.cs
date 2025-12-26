using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Planet9.Source.Managers;
using Planet9.Source.Screens;

namespace Planet9;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _prevKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        // Set resolution
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ScreenManager.Instance.Initialize(Content, GraphicsDevice);
        ParticleManager.Instance.Initialize(GraphicsDevice);
        ScreenManager.Instance.LoadScreen(new TitleScreen());
    }

    protected override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();

        // Toggle Fullscreen with Ctrl+F
        bool isCtrlDown = kstate.IsKeyDown(Keys.LeftControl) || kstate.IsKeyDown(Keys.RightControl);
        if (isCtrlDown && kstate.IsKeyDown(Keys.F) && !_prevKeyboardState.IsKeyDown(Keys.F))
        {
            _graphics.ToggleFullScreen();
        }

        _prevKeyboardState = kstate;

        ScreenManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        ScreenManager.Instance.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
