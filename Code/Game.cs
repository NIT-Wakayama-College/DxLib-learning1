using DxLibDLL;

internal class Game {
    private Player Player;

    public Game() {
        Player = new Player();
    }

    public void Run() {
        while (DX.ProcessMessage() == 0) {
            Render();
        }
    }

    private void Render() {
        DX.ClearDrawScreen();

        Player.Render();

        DX.ScreenFlip();
    }
}
