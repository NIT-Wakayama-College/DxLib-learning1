using DxLibDLL;

internal struct InputState {
    public bool Left;
    public bool Right;
    public bool Jump;
}

internal class Game {
    private static bool _isGameOver;

    private Player Player;
    private InputState InputState;

    public Game() {
        _isGameOver = false;
        Player = new Player();
        InputState = new InputState();
    }

    public void Run() {
        while (DX.ProcessMessage() == 0 && !_isGameOver) {
            Update();
            Render();
        }
    }

    private void Update() {
        InputState = GetCurrentInput();
        Player.Update(InputState);
    }

    private InputState GetCurrentInput() => new InputState {
        Left = DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1,
        Right = DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1,
        Jump = DX.CheckHitKey(DX.KEY_INPUT_SPACE) == 1,
    };

    private void Render() {
        DX.ClearDrawScreen();

        Ground.Render();
        Player.Render();

        DX.ScreenFlip();
    }

    public static void SetGameOver() {
        _isGameOver = true;
    }
}
