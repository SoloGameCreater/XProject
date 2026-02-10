using Framework;

namespace GameplayRuntime
{
    public class GameplayContext : IGameplayContext
    {
        public Game Game { get; }

        public GameplayContext(Game game)
        {
            Game = game;
        }
    }
}
