using DxLibDLL;

using static Constants;

internal class Program {
    static Program() {
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    private static void Main() {
        new Game().Run();

        DX.DxLib_End();
    }

    public static int[] LoadSprites(string filePath, int divX, int divY, int sizeX, int sizeY) {
        int spriteCount = divX * divY;
        int[] sprites = new int[spriteCount];
        DX.LoadDivGraph(ASSET_PATH + filePath, spriteCount, divX, divY, sizeX, sizeY, sprites);
        return sprites;
    }

    public static void DrawEXGraph(int posX, int posY, int sizeX, int sizeY, int GrHandle) {
        DX.DrawExtendGraph(posX, posY, posX + sizeX, posY + sizeY, GrHandle, DX.TRUE);
    }
}
