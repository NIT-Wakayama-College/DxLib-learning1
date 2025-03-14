using static Constants;

internal static class Ground {
    public static void Render() {
        for (int i = 0; i < MAP_HEIGHT; i++) {
            for (int j = 0; j < MAP_WIDTH; j++) {
                int tileIndex = MAP_DATA[i][j];
                if (tileIndex == 0) continue;
                Program.DrawEXGraph(j * GROUND_SIZE, i * GROUND_SIZE, GROUND_SIZE, GROUND_SIZE, GROUND_IMAGES[tileIndex]);
            }
        }
    }
}