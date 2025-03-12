using System.Numerics;

using static Constants;

static class Ground {
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

    static int GetChipParam(Vector2 pos) {
        if (pos.X < 0f || pos.X >= SCREEN_WIDTH) return 0;
        if (pos.Y < 0f) return -1;
        if (pos.Y >= SCREEN_HEIGHT) {
            Game.IsGameOver = true;
            return -1;
        }

        int gridX = (int)((pos.X + Game.CameraOffsetX) / CHIP_SIZE);
        int gridY = (int)(pos.Y / CHIP_SIZE);

        return MAP_DATA[gridY][gridX];
    }
}
