using System.Collections.Generic;
using Planet9.Source.Data;

namespace Planet9.Source.Managers
{
    public class LevelManager
    {
        private static LevelManager _instance;
        public static LevelManager Instance => _instance ??= new LevelManager();

        private List<LevelData> _levels;
        public int CurrentLevelIndex { get; private set; } = 0;
        
        public LevelData CurrentLevel => _levels[CurrentLevelIndex];

        public LevelManager()
        {
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            _levels = new List<LevelData>();
            for (int i = 1; i <= 9; i++)
            {
                var level = new LevelData
                {
                    LevelNumber = i,
                    Name = $"Planet {i}",
                    BackgroundTextureName = i == 2 ? "level2_bg.png" : "level1_bg.png",
                    Waves = new List<WaveData>()
                };

                // Simple progression: More enemies, faster, more health
                level.Waves.Add(new WaveData
                {
                    EnemyCount = 5 + (i * 2),
                    SpawnInterval = System.Math.Max(0.5f, 2.0f - (i * 0.15f)),
                    EnemySpeed = 100f + (i * 20f),
                    EnemyHealth = 1 + (i / 3),
                    EnemyScoreValue = 10 * i,
                    FireChance = 0.05f + (i * 0.05f) // Level 1: 10%, Level 9: 50% chance per second roughly (logic will be in Enemy update)
                });

                _levels.Add(level);
            }
        }

        public bool NextLevel()
        {
            if (CurrentLevelIndex < _levels.Count - 1)
            {
                CurrentLevelIndex++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            CurrentLevelIndex = 0;
        }
    }
}