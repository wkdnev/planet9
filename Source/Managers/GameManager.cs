namespace Planet9.Source.Managers
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance ??= new GameManager();

        public int Score { get; set; }
        public int Money { get; set; }
        public int Lives { get; set; } = 3;
        
        // Upgrades
        public int WeaponLevel { get; set; } = 1;
        public int ShieldLevel { get; set; } = 0;
        public int CurrentShield { get; set; } = 0;

        public void Reset()
        {
            Score = 0;
            Money = 0;
            Lives = 3;
            WeaponLevel = 1;
            ShieldLevel = 0;
            CurrentShield = 0;
        }
    }
}