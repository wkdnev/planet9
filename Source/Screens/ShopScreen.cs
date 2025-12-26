using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Planet9.Source.Managers;

namespace Planet9.Source.Screens
{
    public class ShopScreen : GameScreen
    {
        private SpriteFont _font;
        private int _selectedItem = 0;
        private string[] _items = { "Upgrade Weapon ($100)", "Buy Shield ($50)", "Buy Life ($200)", "Next Level" };
        private int[] _costs = { 100, 50, 200, 0 };
        private KeyboardState _prevKeyboardState;

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Arial");
            _prevKeyboardState = Keyboard.GetState();
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up) && !_prevKeyboardState.IsKeyDown(Keys.Up))
            {
                _selectedItem--;
                if (_selectedItem < 0) _selectedItem = _items.Length - 1;
            }
            if (kstate.IsKeyDown(Keys.Down) && !_prevKeyboardState.IsKeyDown(Keys.Down))
            {
                _selectedItem++;
                if (_selectedItem >= _items.Length) _selectedItem = 0;
            }

            if (kstate.IsKeyDown(Keys.Enter) && !_prevKeyboardState.IsKeyDown(Keys.Enter))
            {
                BuyItem(_selectedItem);
            }

            _prevKeyboardState = kstate;
        }

        private void BuyItem(int index)
        {
            if (index == 3) // Next Level
            {
                if (LevelManager.Instance.NextLevel())
                {
                    ScreenManager.Instance.LoadScreen(new GameplayScreen());
                }
                else
                {
                    // Game Won - Return to Title
                    ScreenManager.Instance.LoadScreen(new TitleScreen());
                }
                return;
            }

            int cost = _costs[index];
            if (GameManager.Instance.Money >= cost)
            {
                GameManager.Instance.Money -= cost;
                switch (index)
                {
                    case 0: GameManager.Instance.WeaponLevel++; break;
                    case 1: GameManager.Instance.ShieldLevel++; break;
                    case 2: GameManager.Instance.Lives++; break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "SHOP - PLANET " + LevelManager.Instance.CurrentLevel.LevelNumber + " COMPLETE", new Vector2(100, 50), Color.White);
            spriteBatch.DrawString(_font, $"Money: ${GameManager.Instance.Money}", new Vector2(100, 80), Color.Gold);
            spriteBatch.DrawString(_font, $"Lives: {GameManager.Instance.Lives}  Weapon Lvl: {GameManager.Instance.WeaponLevel}  Shield Lvl: {GameManager.Instance.ShieldLevel}", new Vector2(100, 110), Color.White);

            for (int i = 0; i < _items.Length; i++)
            {
                Color color = (i == _selectedItem) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _items[i], new Vector2(100, 180 + i * 40), color);
            }
        }
    }
}