using System.Collections.Generic;
using System.IO;
using System;
using System.Numerics;

internal static class Constants
{
    public const string ASSET_PATH = @"..\..\Assets\";

    // Player Constants
    public const int PLAYER_SPEED = 3;
    public const int JUMP_POWER = -20; // Negative for upward force
    public const int GRAVITY_INCREMENT = 1;

    // Map and Tile Constants
    public const int GROUND_SIZE = 32;

    // Screen Constants
    public static readonly Vector2 SCREEN_SIZE;

    // Player Derived Constants
    public static readonly Vector2 PLAYER_SIZE;
    public static readonly Vector2 PLAYER_HALF_SIZE; // Player size / 2

    // Map Dimensions (calculated from MAP_DATA)
    public static readonly int MAP_WIDTH;
    public static readonly int MAP_HEIGHT;

    // Graphics Handles (Loaded after DxLib init)
    public static readonly int[] PLAYER_IMAGES;
    public static readonly int[] GROUND_IMAGES;

    // Map Data
    public static readonly List<List<int>> MAP_DATA;

    static Constants()
    {
        SCREEN_SIZE = new Vector2(640f, 480f);
        PLAYER_SIZE = new Vector2(32f, 64f);
        PLAYER_HALF_SIZE = PLAYER_SIZE / 2f;

        // --- Asset Loading ---
        // IMPORTANT: These LoadSprites calls rely on DxLib being initialized.
        // Ensure Program.cs initializes DxLib BEFORE Game/Constants are heavily used,
        // or move asset loading to an explicit initialization step after DxLib_Init.
        try
        {
            PLAYER_IMAGES = Program.LoadSprites(@"tileset_ramina.png", 3, 2, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y);
            // Assuming ground tiles are 32x32 based on GROUND_SIZE. Adjust if needed.
            GROUND_IMAGES = Program.LoadSprites(@"tileset_ground.png", 5, 3, 16, 16);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading sprites: {ex.Message}");
            // Provide fallback or handle the error appropriately
            PLAYER_IMAGES = new int[0];
            GROUND_IMAGES = new int[0];
        }

        MAP_DATA = LoadMapData(@"tilemap.csv");

        // Calculate map dimensions based on loaded data
        MAP_HEIGHT = MAP_DATA.Count;
        MAP_WIDTH = MAP_HEIGHT > 0 ? MAP_DATA[0].Count : 0;
    }

    private static List<List<int>> LoadMapData(string fileName)
    {
        var mapData = new List<List<int>>();
        string fullPath = Path.Combine(ASSET_PATH, fileName);
        try
        {
            using (StreamReader file = new StreamReader(fullPath))
            {
                int y = 0;
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] strValues = line.Split(',');
                    var intValues = new List<int>();
                    int x = 0;
                    foreach (string strVal in strValues)
                    {
                        if (int.TryParse(strVal.Trim(), out int val))
                        {
                            intValues.Add(val);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Invalid map data '{strVal}' at ({x},{y}) in {fileName}. Using 0.");
                            intValues.Add(0); // Default to empty space
                        }
                        x++;
                    }
                    mapData.Add(intValues);
                    y++;
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: Map file not found at {fullPath}");
            // Return an empty map or default map
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading map data from {fullPath}: {ex.Message}");
        }
        return mapData;
    }

    /// <summary>
    /// Checks if the tile at the given grid coordinates is solid (collidable).
    /// </summary>
    /// <param name="tileX">Tile X index.</param>
    /// <param name="tileY">Tile Y index.</param>
    /// <returns>True if the tile is solid, False otherwise.</returns>
    public static bool IsSolidTile(int tileX, int tileY)
    {
        // Consider out-of-bounds solid to prevent escaping the map
        if (tileX < 0 || tileX >= MAP_WIDTH || tileY < 0 || tileY >= MAP_HEIGHT)
        {
            return true;
        }
        // Tile index 0 is considered empty space, others are solid
        // Adjust this logic if your map format uses different conventions
        return MAP_DATA[tileY][tileX] != 0;
    }
}
