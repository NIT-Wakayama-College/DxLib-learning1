using DxLibDLL;

internal class Game {
    public void Run() {
        while (DX.ProcessMessage() == 0) {
            Render();
        }
    }

    private void Render() {
        DX.ClearDrawScreen();
        DX.DrawString(100, 100, "Hello World", DX.GetColor(255, 255, 255));
        DX.ScreenFlip();
    }
}
