using DxLibDLL;
using System;

public struct InputState {
    public bool Left;
    public bool Right;
    public bool Jump;
}

static class Game {
    public static readonly int GROUND_Y = 300;

    static readonly int TIMER_INTERVAL = 16;

    static int _timer;

    static Player _player = new Player();
    static InputState _currentInput = new InputState();

    private static InputState GetCurrentInput() => new InputState {
        Left = DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1,
        Right = DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1,
        Jump = DX.CheckHitKey(DX.KEY_INPUT_SPACE) == 1
    };

    public static void Run() {
        _timer = DX.GetNowCount();

        while (DX.ProcessMessage() == 0) {
            Update();
            Render();

            _timer += TIMER_INTERVAL;
            DX.WaitTimer(Math.Max(1, _timer - DX.GetNowCount()));
        }
    }

    public static void Update() {
        _currentInput = GetCurrentInput();
        _player.Update(_currentInput);
    }

    public static void Render() {
        DX.ClearDrawScreen();

        Ground.Render();

        _player.Render();

        DX.ScreenFlip();
    }
}

static class Program {
    public static readonly int SCREEN_X = 640, SCREEN_Y = 480;

    public static readonly uint COLOR_RED = DX.GetColor(255, 0, 0);
    public static readonly uint COLOR_WHITE = DX.GetColor(255, 255, 255);

    static void Main() {
        Init();
        Game.Run();
        DX.DxLib_End();
    }

    static void Init() {
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    public static void DrawBox(int posX, int posY, int sizeX, int sizeY, uint color) =>
        DX.DrawBox(posX, posY, posX + sizeX, posY + sizeY, color, DX.TRUE);
}
