using System.Collections.Generic;

namespace Planet9.Source.Data
{
    public class WaveData
    {
        public int EnemyCount;
        public float SpawnInterval;
        public float EnemySpeed;
        public int EnemyHealth;
        public int EnemyScoreValue;
        public float FireChance; // Probability to fire per second
    }

    public class LevelData
    {
        public int LevelNumber;
        public string Name;
        public string BackgroundTextureName;
        public List<WaveData> Waves;
    }
}