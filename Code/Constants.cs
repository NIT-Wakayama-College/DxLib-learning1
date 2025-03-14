using System.Collections.Generic;
using System.IO;
using System;
using System.Numerics;

internal static class Constants {
    public const string ASSET_PATH = @"..\..\Assets\";

    public const int PLAYER_SPEED = 3;
    public const int JUMP_POWER = -20;
    public const int GRAVITY_INCREMENT = 1;

    public const int GROUND_SIZE = 32;

    public static readonly int MAP_WIDTH;
    public static readonly int MAP_HEIGHT;

    public static readonly Vector2 SCREEN_SIZE;
    public static readonly Vector2 PLAYER_SIZE;

    public static readonly int[] PLAYER_IMAGES;
    public static readonly int[] GROUND_IMAGES;

    public static readonly List<List<int>> MAP_DATA;

    static Constants() {
        SCREEN_SIZE = new Vector2(640f, 480f);
        PLAYER_SIZE = new Vector2(32f, 64f);

        PLAYER_IMAGES = Program.LoadSprites(@"tileset_ramina.png", 3, 2, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y);
        GROUND_IMAGES = Program.LoadSprites(@"tileset_ground.png", 5, 3, 16, 16);

        MAP_DATA = LoadMapData(@"tilemap.csv");

        MAP_WIDTH = MAP_DATA[0].Count;
        MAP_HEIGHT = MAP_DATA.Count;
    }

    private static List<List<int>> LoadMapData(string fileName) {
        var mapData = new List<List<int>>();
        using (StreamReader file = new StreamReader(ASSET_PATH + fileName)) {
            while (!file.EndOfStream) {
                string line = file.ReadLine();
                string[] strValues = line.Split(',');
                int[] intValues = Array.ConvertAll(strValues, int.Parse);
                mapData.Add(new List<int>(intValues));
            }
        }
        return mapData;
    }
}
