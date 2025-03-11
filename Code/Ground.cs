static class Ground {
    const int CHIP_SIZE = 64;

    const int MAP_WIDTH = 10;
    const int MAP_HEIGHT = 8;

    static int[,] MapData = new int[MAP_HEIGHT, MAP_WIDTH]{
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 } ,
        { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 } ,
        { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 } ,
        { 1, 1, 0, 1, 1, 0, 0, 0, 1, 1 } ,
        { 1, 1, 1, 1, 1, 0, 0, 0, 1, 1 } ,
        { 1, 1, 0, 1, 0, 0, 0, 0, 1, 1 } ,
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } ,
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    public static void Render() {
        for (int i = 0; i < MAP_HEIGHT; i++) {
            for (int j = 0; j < MAP_WIDTH; j++) {
                if (MapData[i, j] == 0) {
                    Program.DrawBox(j * CHIP_SIZE, i * CHIP_SIZE, CHIP_SIZE, CHIP_SIZE, Program.COLOR_RED);
                }
            }
        }
    }

    public static int CheckCollisionHorizontal(int x, int y, int dx, int size) {
        int left = x - size / 2 + dx;
        int right = x + size / 2 + dx;

        if (dx < 0) {
            if (IsWall(left, y))
                return (left / CHIP_SIZE + 1) * CHIP_SIZE + size / 2;
        } else if (dx > 0) {
            if (IsWall(right, y))
                return (right / CHIP_SIZE) * CHIP_SIZE - size / 2;
        }
        return x + dx;
    }

    public static int CheckCollisionVertical(int x, int y, int dy, int size, ref int _gravity, ref bool _isJumping) {
        int bottom = y + size / 2 + dy;
        int top = y - size / 2 + dy;

        if (dy < 0) {
            if (IsWall(x, top))
                return (top / CHIP_SIZE + 1) * CHIP_SIZE + size / 2;
        } else if (dy > 0) {
            if (IsWall(x, bottom)) {
                _gravity = 0;
                _isJumping = false;
                return (bottom / CHIP_SIZE) * CHIP_SIZE - size / 2;
            }
        }
        return y + dy;
    }

    public static bool IsWall(int x, int y) {
        return GetChipParam(x, y) != 1;
    }

    public static int GetChipParam(int x, int y) {
        if (x < 0 || x >= Program.SCREEN_X) return 0;
        if (y < 0 || y >= Program.SCREEN_Y) return 0;

        int gridX = x / CHIP_SIZE;
        int gridY = y / CHIP_SIZE;

        return MapData[gridY, gridX];
    }
}
