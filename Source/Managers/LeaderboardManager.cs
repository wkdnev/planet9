using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Planet9.Source.Managers
{
    public class HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public class LeaderboardManager
    {
        private static LeaderboardManager _instance;
        public static LeaderboardManager Instance => _instance ??= new LeaderboardManager();

        private string _filePath = "leaderboard.json";
        public List<HighScore> HighScores { get; private set; }

        public LeaderboardManager()
        {
            Load();
        }

        public void AddScore(string name, int score)
        {
            HighScores.Add(new HighScore { Name = name, Score = score });
            HighScores = HighScores.OrderByDescending(s => s.Score).Take(50).ToList();
            Save();
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(HighScores);
            File.WriteAllText(_filePath, json);
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                HighScores = JsonSerializer.Deserialize<List<HighScore>>(json);
            }
            else
            {
                HighScores = new List<HighScore>();
            }
        }
    }
}