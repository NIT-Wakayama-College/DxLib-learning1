using DxLibDLL;

internal struct InputState {
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;
}

internal class Game {
    private Player Player;
    private InputState InputState;

    public Game() {
        Player = new Player();
    }

    public void Run() {
        while (DX.ProcessMessage() == 0) {
            Update();
            Render();
        }
    }

    private void Update() {
        InputState = GetCurrentInput();
        Player.Update(InputState);
    }

    private InputState GetCurrentInput() => new InputState {
        Up = DX.CheckHitKey(DX.KEY_INPUT_UP) == 1,
        Down = DX.CheckHitKey(DX.KEY_INPUT_DOWN) == 1,
        Left = DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1,
        Right = DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1,
    };

    private void Render() {
        DX.ClearDrawScreen();

        Player.Render();

        DX.ScreenFlip();
    }
}
