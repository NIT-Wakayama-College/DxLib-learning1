using DxLibDLL;

class Game
{
    public void Update()
    {
        // 今後のゲームの更新処理 (キャラクターの移動など) をここに追加
    }

    public void Render()
    {
        DX.ClearDrawScreen();
        DX.DrawString(100, 100, "Hello World", DX.GetColor(255, 255, 255));
        DX.ScreenFlip();
    }

    public void Run()
    {
        while (DX.ProcessMessage() == 0)
        {
            Update();
            Render();
        }
    }
}

static class Program
{
    static Program()
    {
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    static void Main(string[] args)
    {
        Game Game = new Game();
        Game.Run();

        DX.DxLib_End();
    }
}
