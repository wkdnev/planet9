using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using Planet9.Source.Managers;

namespace Planet9.Source.Entities
{
    public class Player
    {
        public Vector2 Position;
        public Texture2D Texture;
        private Texture2D _textureIdle;
        private Texture2D _textureMoving;
        private SoundEffect _shootSound;
        public float Speed = 400f;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        private GraphicsDevice _graphicsDevice;
        private Texture2D _bulletTexture;
        private float _fireTimer = 0f;
        public float FireRate = 0.2f;

        // Invulnerability
        private float _invulnerabilityTimer = 0f;
        private bool _isVisible = true;
        private float _flashTimer = 0f;
        private const float InvulnerabilityDuration = 1.0f;
        private const float FlashInterval = 0.1f;

        public bool IsInvulnerable => _invulnerabilityTimer > 0;

        public Player(GraphicsDevice graphicsDevice, Vector2 startPosition, SoundEffect shootSound)
        {
            _graphicsDevice = graphicsDevice;
            Position = startPosition;
            _shootSound = shootSound;
            
            // Load ship texture
            using (var stream = TitleContainer.OpenStream("Content/player_ship.png"))
            {
                _textureMoving = Texture2D.FromStream(_graphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/player_ship_non_move.png"))
            {
                _textureIdle = Texture2D.FromStream(_graphicsDevice, stream);
            }
            Texture = _textureIdle;

            // Create bullet texture
            _bulletTexture = new Texture2D(_graphicsDevice, 5, 15);
            Color[] bulletData = new Color[5 * 15];
            for (int i = 0; i < bulletData.Length; ++i) bulletData[i] = Color.Yellow;
            _bulletTexture.SetData(bulletData);
        }

        public void StartInvulnerability()
        {
            _invulnerabilityTimer = InvulnerabilityDuration;
            _flashTimer = 0f;
            _isVisible = true;
        }

        public void Update(GameTime gameTime, List<Bullet> bullets)
        {
            var kstate = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Invulnerability Logic
            if (_invulnerabilityTimer > 0)
            {
                _invulnerabilityTimer -= dt;
                _flashTimer -= dt;

                if (_flashTimer <= 0)
                {
                    _isVisible = !_isVisible;
                    _flashTimer = FlashInterval;
                }
            }
            else
            {
                _isVisible = true;
            }

            bool isMoving = false;

            if (kstate.IsKeyDown(Keys.Left))
            {
                Position.X -= Speed * dt;
                isMoving = true;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                Position.X += Speed * dt;
                isMoving = true;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                Position.Y -= Speed * dt;
                isMoving = true;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                Position.Y += Speed * dt;
                isMoving = true;
            }

            // Update texture based on movement
            Texture = isMoving ? _textureMoving : _textureIdle;

            // Clamp to screen
            Position.X = MathHelper.Clamp(Position.X, 0, _graphicsDevice.Viewport.Width - Texture.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, _graphicsDevice.Viewport.Height - Texture.Height);

            // Shooting
            _fireTimer -= dt;
            if (kstate.IsKeyDown(Keys.A) && _fireTimer <= 0)
            {
                _shootSound?.Play();
                int weaponLevel = GameManager.Instance.WeaponLevel;
                float currentFireRate = (weaponLevel >= 4) ? 0.1f : FireRate;

                if (weaponLevel == 1)
                {
                    Vector2 bulletPos = new Vector2(Position.X + Texture.Width / 2 - _bulletTexture.Width / 2, Position.Y);
                    Vector2 bulletVel = new Vector2(0, -600f); // Upwards
                    bullets.Add(new Bullet(_bulletTexture, bulletPos, bulletVel));
                }
                else if (weaponLevel == 2)
                {
                    bullets.Add(new Bullet(_bulletTexture, Position + new Vector2(0, 0), new Vector2(0, -600f)));
                    bullets.Add(new Bullet(_bulletTexture, Position + new Vector2(Texture.Width - 5, 0), new Vector2(0, -600f)));
                }
                else
                {
                    Vector2 center = Position + new Vector2(Texture.Width / 2 - 2, 0);
                    bullets.Add(new Bullet(_bulletTexture, center, new Vector2(0, -600f)));
                    bullets.Add(new Bullet(_bulletTexture, center, new Vector2(-150, -600f)));
                    bullets.Add(new Bullet(_bulletTexture, center, new Vector2(150, -600f)));
                }

                _fireTimer = currentFireRate;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
            {
                Color color = IsInvulnerable ? Color.Red * 0.8f : Color.White;
                spriteBatch.Draw(Texture, Position, color);
            }
        }
    }
}