namespace AppleFrenzy.Core
{
    /// <summary>
    ///     Represents the various 'Game Events' triggered throughout the game.
    /// </summary>
    public class GameEvents
    {
        public const string APPLE_CAUGHT = "APPLE_CAUGHT"; // -> Triggered whenever an Apple is caught.
        public const string APPLE_DROPPED = "APPLE_DROPPED"; // -> Triggered whenever a good Apple is dropped.
        public const string LIFE_ONE_UP = "LIFE_ONE_UP"; // -> Triggered whenever 100 good Apples are caught.
        public const string LIFE_ONE_DOWN = "LIFE_ONE_DOWN"; // -> Triggered whenever a life is lost.

        public const string SCORE_UPDATE = "SCORE_UPDATE"; // -> Triggered whenever the player's score changes.
        public const string COMBO_UPDATE = "COMBO_UPDATE"; // -> Triggered whenever the combo changes.
        public const string GAME_OVER = "GAME_OVER"; // -> Triggered when the game ends.

        public const string INC_DIFFICULTY = "INC_DIFFICULTY"; // -> Triggered whenever the player hits a level's score threshold.
    };
}
