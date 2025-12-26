using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Planet9.Source.Managers;
using Planet9.Source.Entities;
using Planet9.Source.Data;
using System.Collections.Generic;
using System.IO;

namespace Planet9.Source.Screens
{
    public class GameplayScreen : GameScreen
    {
        private SpriteFont _font;
        private Player _player;
        private List<Bullet> _bullets;
        private List<Bullet> _enemyBullets;
        private List<Enemy> _enemies;
        private List<MoneyDrop> _moneyDrops;
        private Texture2D _enemyTexture;
        private Texture2D _moneyTexture;
        private Texture2D _enemyBulletTexture;
        private Texture2D _level1Background;
        private Song _level1Music;
        private Song _level2Music;
        private System.Random _random;

        // Level Management
        private int _currentWaveIndex = 0;
        private int _enemiesSpawnedInWave = 0;
        private float _enemySpawnTimer = 0f;
        private bool _levelComplete = false;
        private float _levelCompleteTimer = 0f;

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Arial");
            _random = new System.Random();
            
            // Center the player at the bottom
            Vector2 startPos = new Vector2(
                _graphicsDevice.Viewport.Width / 2 - 15, 
                _graphicsDevice.Viewport.Height - 50
            );
            _player = new Player(_graphicsDevice, startPos);
            
            _bullets = new List<Bullet>();
            _enemyBullets = new List<Bullet>();
            _enemies = new List<Enemy>();
            _moneyDrops = new List<MoneyDrop>();

            // Load enemy texture
            using (var stream = TitleContainer.OpenStream("Content/alien1.png"))
            {
                _enemyTexture = Texture2D.FromStream(_graphicsDevice, stream);
            }

            // Load Level 1 Background
            try
            {
                using (var stream = TitleContainer.OpenStream("Content/level1_bg.png"))
                {
                    _level1Background = Texture2D.FromStream(_graphicsDevice, stream);
                }
            }
            catch
            {
                // Fallback if file missing
            }

            // Load Level 1 Music
            try
            {
                _level1Music = _content.Load<Song>("level1-music");
            }
            catch
            {
                // Fallback
            }

            // Load Level 2 Music
            try
            {
                _level2Music = _content.Load<Song>("level2-music");
            }
            catch
            {
                // Fallback
            }

            // Create money texture
            _moneyTexture = new Texture2D(_graphicsDevice, 15, 15);
            Color[] moneyData = new Color[15 * 15];
            for (int i = 0; i < moneyData.Length; ++i) moneyData[i] = Color.Gold;
            _moneyTexture.SetData(moneyData);

            // Create enemy bullet texture
            _enemyBulletTexture = new Texture2D(_graphicsDevice, 12, 12);
            Color[] ebData = new Color[12 * 12];
            for (int i = 0; i < ebData.Length; ++i) ebData[i] = Color.OrangeRed;
            _enemyBulletTexture.SetData(ebData);

            StartLevel();
        }

        private void StartLevel()
        {
            _currentWaveIndex = 0;
            _enemiesSpawnedInWave = 0;
            _enemySpawnTimer = 2.0f;
            _levelComplete = false;
            _bullets.Clear();
            _enemyBullets.Clear();
            _enemies.Clear();
            _moneyDrops.Clear();
            
            // Refill shield at start of level based on level
            GameManager.Instance.CurrentShield = GameManager.Instance.ShieldLevel;

            // Play music based on level
            if (LevelManager.Instance.CurrentLevel.LevelNumber == 1 && _level1Music != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_level1Music);
            }
            else if (LevelManager.Instance.CurrentLevel.LevelNumber == 2 && _level2Music != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_level2Music);
            }
            else
            {
                // Stop music for other levels (or play different music)
                MediaPlayer.Stop();
            }
        }

        public override void UnloadContent()
        {
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenManager.Instance.LoadScreen(new TitleScreen());
            }

            if (_levelComplete)
            {
                _levelCompleteTimer -= dt;
                if (_levelCompleteTimer <= 0)
                {
                    // Go to Shop Screen
                    ScreenManager.Instance.LoadScreen(new ShopScreen());
                }
                return; // Skip other updates
            }

            ParticleManager.Instance.Update(gameTime);
            _player.Update(gameTime, _bullets);

            // Update Bullets
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Update(gameTime);
                if (_bullets[i].Position.Y < -50 || !_bullets[i].IsActive)
                {
                    _bullets.RemoveAt(i);
                }
            }

            // Level & Wave Logic
            var levelData = LevelManager.Instance.CurrentLevel;
            if (_currentWaveIndex < levelData.Waves.Count)
            {
                var wave = levelData.Waves[_currentWaveIndex];
                
                if (_enemiesSpawnedInWave < wave.EnemyCount)
                {
                    _enemySpawnTimer -= dt;
                    if (_enemySpawnTimer <= 0)
                    {
                        Vector2 spawnPos = new Vector2(
                            _random.Next(0, _graphicsDevice.Viewport.Width - 40),
                            -40
                        );
                        _enemies.Add(new Enemy(_enemyTexture, spawnPos, wave.EnemyHealth, wave.EnemySpeed, wave.EnemyScoreValue, wave.FireChance));
                        _enemiesSpawnedInWave++;
                        _enemySpawnTimer = wave.SpawnInterval;
                    }
                }
                else if (_enemies.Count == 0)
                {
                    _currentWaveIndex++;
                    _enemiesSpawnedInWave = 0;
                }
            }
            else
            {
                // All waves complete
                _levelComplete = true;
                _levelCompleteTimer = 3.0f;
            }

            // Update Enemies
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, _enemies, (pos) => 
                {
                    // Enemy Shoot Callback
                    Vector2 vel = new Vector2(0, 300f); // Bullet speed down
                    _enemyBullets.Add(new Bullet(_enemyBulletTexture, pos, vel));
                });

                if (_enemies[i].Position.Y > _graphicsDevice.Viewport.Height || !_enemies[i].IsActive)
                {
                    _enemies.RemoveAt(i);
                }
            }

            // Update Enemy Bullets
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                _enemyBullets[i].Update(gameTime);
                if (_enemyBullets[i].Position.Y > _graphicsDevice.Viewport.Height || !_enemyBullets[i].IsActive)
                {
                    _enemyBullets.RemoveAt(i);
                }
            }

            // Update MoneyDrops
            for (int i = _moneyDrops.Count - 1; i >= 0; i--)
            {
                _moneyDrops[i].Update(gameTime);
                if (_moneyDrops[i].Position.Y > _graphicsDevice.Viewport.Height || !_moneyDrops[i].IsActive)
                {
                    _moneyDrops.RemoveAt(i);
                }
            }

            // Collision Detection
            foreach (var bullet in _bullets)
            {
                foreach (var enemy in _enemies)
                {
                    if (bullet.IsActive && enemy.IsActive && bullet.Bounds.Intersects(enemy.Bounds))
                    {
                        bullet.IsActive = false;
                        enemy.Health--;
                        if (enemy.Health <= 0) 
                        {
                            enemy.IsActive = false;
                            ParticleManager.Instance.SpawnExplosion(enemy.Position + new Vector2(20, 20), Color.Orange);
                            GameManager.Instance.Score += enemy.ScoreValue;
                            if (_random.NextDouble() < 0.3) // 30% chance
                            {
                                _moneyDrops.Add(new MoneyDrop(_moneyTexture, enemy.Position, 10));
                            }
                        }
                    }
                }
            }

            // Collision Player-Money
            foreach (var drop in _moneyDrops)
            {
                if (drop.IsActive && drop.Bounds.Intersects(_player.Bounds))
                {
                    drop.IsActive = false;
                    GameManager.Instance.Money += drop.Value;
                }
            }

            // Collision Enemy-Player
            foreach (var enemy in _enemies)
            {
                if (enemy.IsActive && enemy.Bounds.Intersects(_player.Bounds))
                {
                    enemy.IsActive = false;
                    ParticleManager.Instance.SpawnExplosion(enemy.Position, Color.Red);
                    
                    TakeDamage();
                }
            }

            // Collision EnemyBullet-Player
            foreach (var bullet in _enemyBullets)
            {
                if (bullet.IsActive && bullet.Bounds.Intersects(_player.Bounds))
                {
                    bullet.IsActive = false;
                    ParticleManager.Instance.SpawnExplosion(_player.Position + new Vector2(15, 15), Color.Red);
                    TakeDamage();
                }
            }
        }

        private void TakeDamage()
        {
            if (_player.IsInvulnerable) return;

            _player.StartInvulnerability();

            if (GameManager.Instance.CurrentShield > 0)
            {
                GameManager.Instance.CurrentShield--;
            }
            else
            {
                GameManager.Instance.Lives--;
                if (GameManager.Instance.Lives <= 0)
                {
                    LeaderboardManager.Instance.AddScore("Player", GameManager.Instance.Score);
                    ScreenManager.Instance.LoadScreen(new TitleScreen());
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Background for Level 1
            if (LevelManager.Instance.CurrentLevel.LevelNumber == 1 && _level1Background != null)
            {
                spriteBatch.Draw(_level1Background, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            }

            _player.Draw(spriteBatch);
            
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var bullet in _enemyBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var drop in _moneyDrops)
            {
                drop.Draw(spriteBatch);
            }

            ParticleManager.Instance.Draw(spriteBatch);

            // HUD
            spriteBatch.DrawString(_font, $"Level: {LevelManager.Instance.CurrentLevel.LevelNumber} - {LevelManager.Instance.CurrentLevel.Name}", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(_font, $"Score: {GameManager.Instance.Score}  Money: ${GameManager.Instance.Money}  Lives: {GameManager.Instance.Lives}  Shield: {GameManager.Instance.CurrentShield}/{GameManager.Instance.ShieldLevel}", new Vector2(20, 50), Color.White);

            if (_levelComplete)
            {
                string msg = "LEVEL COMPLETE!";
                Vector2 size = _font.MeasureString(msg);
                Vector2 center = new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);
                spriteBatch.DrawString(_font, msg, center - size / 2, Color.Green);
            }
        }
    }
}