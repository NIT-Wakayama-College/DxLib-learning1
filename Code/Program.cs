using DxLibDLL;
using System;
using System.Collections.Generic;
using System.IO;

using static Constants;

struct InputState {
    public bool Left;
    public bool Right;
    public bool Jump;
}

static class Constants {
    public const string ASSET_PATH = @"..\..\Assets\";

    public const int SCREEN_WIDTH = 640;
    public const int SCREEN_HEIGHT = 480;

    public const int TIMER_INTERVAL = 16;

    public const int PLAYER_SIZE_X = 32;
    public const int PLAYER_SIZE_Y = 64;
    public const int PLAYER_SPEED = 3;
    public const int JUMP_POWER = -20;
    public const int GRAVITY_INCREMENT = 1;

    public const int CHIP_SIZE = 32;

    public static readonly uint COLOR_RED;
    public static readonly uint COLOR_WHITE;

    public static readonly int MAP_WIDTH;
    public static readonly int MAP_HEIGHT;

    public static readonly int[] PLAYER_IMAGES;
    public static readonly int[] GROUND_IMAGES;

    public static readonly List<List<int>> MAP_DATA;

    static Constants() {
        COLOR_RED = DX.GetColor(255, 0, 0);
        COLOR_WHITE = DX.GetColor(255, 255, 255);

        PLAYER_IMAGES = Program.LoadSprites(@"tileset_ramina.png", 3, 2, PLAYER_SIZE_X, PLAYER_SIZE_Y);
        GROUND_IMAGES = Program.LoadSprites(@"tileset_ground.png", 25, 23, 16, 16);

        MAP_DATA = new List<List<int>>();
        using (StreamReader file = new StreamReader(ASSET_PATH + @"tilemap.csv")) {
            while (!file.EndOfStream) {
                string line = file.ReadLine();

                string[] strValues = line.Split(',');
                int[] intValues = Array.ConvertAll(strValues, int.Parse);

                MAP_DATA.Add(new List<int>(intValues));
            }
        }

        MAP_WIDTH = MAP_DATA[0].Count;
        MAP_HEIGHT = MAP_DATA.Count;
    }
}

static class Game {
    public static bool IsGameOver = false;
    public static int CameraOffsetX = 0;

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
        Game.Run();
        DX.DxLib_End();
    }

    static Program() {
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    public static int[] LoadSprites(string filePath, int divX, int divY, int sizeX, int sizeY) {
        int spriteCount = divX * divY;
        int[] sprites = new int[spriteCount];
        DX.LoadDivGraph(ASSET_PATH + filePath, spriteCount, divX, divY, sizeX, sizeY, sprites);
        return sprites;
    }

    public static void DrawEXGraph(int posX, int posY, int sizeX, int sizeY, int GrHandle) =>
        DX.DrawExtendGraph(posX, posY, posX + sizeX, posY + sizeY, GrHandle, DX.TRUE);
}
