using DxLibDLL;
using System;

using static Constants;

struct InputState {
    public bool Left;
    public bool Right;
    public bool Jump;
}

class Constants {
    public const int SCREEN_WIDTH = 640;
    public const int SCREEN_HEIGHT = 480;

    public const int TIMER_INTERVAL = 16;

    public const int CHIP_SIZE = 64;
    public const int MAP_WIDTH = 10;
    public const int MAP_HEIGHT = 8;

    public const int PLAYER_SIZE = 32;

    public const int PLAYER_SPEED = 3;
    public const int JUMP_POWER = -20;
    public const int GRAVITY_INCREMENT = 1;

    public static readonly uint COLOR_RED = DX.GetColor(255, 0, 0);
    public static readonly uint COLOR_WHITE = DX.GetColor(255, 255, 255);

    public static readonly int[,] MAP_DATA = new int[MAP_HEIGHT, MAP_WIDTH]{
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 } ,
        { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 } ,
        { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 } ,
        { 1, 1, 0, 1, 1, 0, 0, 0, 1, 1 } ,
        { 1, 1, 1, 1, 1, 0, 0, 0, 1, 1 } ,
        { 1, 1, 0, 1, 0, 0, 0, 0, 1, 1 } ,
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } ,
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };
}

static class Game {
    static int _timer;

    static Player _player = new Player();
    static InputState _currentInput = new InputState();

    static InputState GetCurrentInput() => new InputState {
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

    static void Update() {
        _currentInput = GetCurrentInput();
        _player.Update(_currentInput);
    }

    static void Render() {
        DX.ClearDrawScreen();

        Ground.Render();

        _player.Render();

        DX.ScreenFlip();
    }
}

static class Program {
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
