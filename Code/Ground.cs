using System.Numerics;

using static Constants;

internal static class Ground {
    public static void Render() {
        for (int i = 0; i < MAP_HEIGHT; i++) {
            for (int j = 0; j < MAP_WIDTH; j++) {
                int tileIndex = MAP_DATA[i][j];
                if (tileIndex == -1) continue;
                Program.DrawEXGraph(j * CHIP_SIZE - Game.CameraOffsetX, i * CHIP_SIZE, CHIP_SIZE, CHIP_SIZE, GROUND_IMAGES[tileIndex]);
            }
        }
    }

    public static bool IsWall(Vector2 pos) {
        return GetChipParam(pos) != -1;
    }

    private static int GetChipParam(Vector2 pos) {
        if (IsOutOfBounds(pos)) return 0;
        if (IsAboveScreen(pos)) return -1;
        if (IsBelowScreen(pos)) {
            Game.IsGameOver = true;
            return -1;
        }

        int gridX = (int)((pos.X + Game.CameraOffsetX) / CHIP_SIZE);
        int gridY = (int)(pos.Y / CHIP_SIZE);

        return MAP_DATA[gridY][gridX];
    }

    private static bool IsOutOfBounds(Vector2 pos) {
        return pos.X < 0f || pos.X >= SCREEN_WIDTH;
    }

    private static bool IsAboveScreen(Vector2 pos) {
        return pos.Y < 0f;
    }

    private static bool IsBelowScreen(Vector2 pos) {
        return pos.Y >= SCREEN_HEIGHT;
    }
}
