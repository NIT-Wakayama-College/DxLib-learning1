using DxLibDLL;
using System; // For Math
using System.Numerics; // For Vector2
using static Constants; // Access constants like GROUND_SIZE, MAP_DATA etc.

internal static class Ground
{
    /// <summary>
    /// Renders the visible portion of the map based on the player's position.
    /// </summary>
    /// <param name="playerPosition">The player's current world coordinates.</param>
    public static void Render(Vector2 playerPosition)
    {
        // Calculate the camera's top-left world coordinates based on the player
        // Player is centered horizontally on screen
        float cameraX = playerPosition.X - SCREEN_SIZE.X / 2f;
        // No vertical scrolling in this implementation
        float cameraY = 0f;

        // Determine the range of tiles visible on screen
        int startTileX = (int)Math.Floor(cameraX / GROUND_SIZE);
        int endTileX = (int)Math.Floor((cameraX + SCREEN_SIZE.X) / GROUND_SIZE) + 1; // +1 ensures tiles partially on screen are drawn
        int startTileY = 0; // No vertical scroll
        int endTileY = MAP_HEIGHT; // Draw all vertical tiles

        // Clamp the tile range to the map boundaries
        startTileX = Math.Max(0, startTileX);
        endTileX = Math.Min(MAP_WIDTH, endTileX); // Use MAP_WIDTH (exclusive)
        startTileY = Math.Max(0, startTileY);
        endTileY = Math.Min(MAP_HEIGHT, endTileY); // Use MAP_HEIGHT (exclusive)

        // Loop through the visible tiles and draw them
        for (int y = startTileY; y < endTileY; y++)
        {
            for (int x = startTileX; x < endTileX; x++)
            {
                int tileIndex = MAP_DATA[y][x];

                // Skip empty tiles (index 0) or invalid indices
                if (tileIndex <= 0 || tileIndex >= GROUND_IMAGES.Length) continue;

                int imageHandle = GROUND_IMAGES[tileIndex];
                // Skip if the image failed to load
                if (imageHandle < 0) continue;

                // Calculate the screen coordinates for the tile
                float screenX = x * GROUND_SIZE - cameraX;
                float screenY = y * GROUND_SIZE - cameraY;

                // Draw the tile using DxLib (Directly calling DX function)
                DX.DrawExtendGraph(
                    (int)screenX,
                    (int)screenY,
                    (int)(screenX + GROUND_SIZE),
                    (int)(screenY + GROUND_SIZE),
                    imageHandle,
                    DX.TRUE // Enable transparency
                );
            }
        }
    }
}
