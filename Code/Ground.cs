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
}
