using DxLibDLL;

static class Game {
    public static void Update() {
        // 今後のゲームの更新処理 (キャラクターの移動など) をここに追加
    }

    public static void Render() {
        DX.ClearDrawScreen();
        DX.DrawString(100, 100, "Hello World", DX.GetColor(255, 255, 255));
        DX.ScreenFlip();
    }

    public static void Run() {
        while (DX.ProcessMessage() == 0) {
            Update();
            Render();
        }
    }
}

static class Program {
    static void Init() {
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    static void Main(string[] args) {
        Init();
        Game.Run();
        DX.DxLib_End();
    }
}
