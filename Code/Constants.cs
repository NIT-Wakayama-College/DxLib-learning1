using System.Numerics;

internal static class Constants {
    public const string ASSET_PATH = @"..\..\Assets\";

    public const int PLAYER_SPEED = 3;
    public const int JUMP_POWER = -20;
    public const int GRAVITY_INCREMENT = 1;

    public static readonly Vector2 SCREEN_SIZE;
    public static readonly Vector2 PLAYER_SIZE;

    public static readonly int[] PLAYER_IMAGES;

    static Constants() {
        SCREEN_SIZE = new Vector2(640f, 480f);
        PLAYER_SIZE = new Vector2(32f, 64f);

        PLAYER_IMAGES = Program.LoadSprites(@"tileset_ramina.png", 3, 2, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y);
    }
}
