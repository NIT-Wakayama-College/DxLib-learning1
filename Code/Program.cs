using DxLibDLL;

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
}
