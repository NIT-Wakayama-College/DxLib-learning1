using DxLibDLL;
using System; // For Path, Console
using static Constants; // Access ASSET_PATH

internal class Program
{
    static Program()
    {
        // --- DxLib Initialization ---
        // Set windowed mode
        DX.ChangeWindowMode(DX.TRUE);
        // Set window title
        DX.SetMainWindowText("Simple Platformer");
        // Set Graph Mode (Optional, can fine-tune resolution if needed)
        // DX.SetGraphMode((int)Constants.SCREEN_SIZE.X, (int)Constants.SCREEN_SIZE.Y, 32);

        // Initialize DxLib
        if (DX.DxLib_Init() == -1)
        {
            // Initialization failed, handle error (e.g., show message box, exit)
            Console.WriteLine("Error: Failed to initialize DxLib.");
            Environment.Exit(1); // Exit the application
        }

        // Set the draw target to the back buffer (for double buffering)
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);

        // --- Ensure Assets are Loaded AFTER DxLib Init ---
        // The static constructor for Constants likely ran before DxLib_Init.
        // If LoadSprites was called there, it might have failed silently.
        // It's safer to have an explicit asset loading step here or ensure
        // Constants' static constructor is triggered *after* DxLib_Init.
        // For simplicity, we assume the static constructor worked or handle errors there.
        // A more robust approach:
        // Constants.LoadAssets(); // Create a method in Constants to load assets
    }

    private static void Main()
    {
        // Create and run the game instance
        try
        {
            new Game().Run();
        }
        catch (Exception ex)
        {
            // Catch potential unhandled exceptions during the game loop
            Console.WriteLine($"An error occurred: {ex.Message}\n{ex.StackTrace}");
            // Optionally show an error message to the user
        }
        finally
        {
            // Clean up DxLib resources
            DX.DxLib_End();
        }
    }

    /// <summary>
    /// Loads a sprite sheet and divides it into individual sprite handles.
    /// Moved here to be explicitly called after DxLib init if needed,
    /// but currently called by Constants' static constructor.
    /// </summary>
    public static int[] LoadSprites(string filePath, int divX, int divY, int sizeX, int sizeY)
    {
        int spriteCount = divX * divY;
        int[] sprites = new int[spriteCount];
        string fullPath = System.IO.Path.Combine(ASSET_PATH, filePath);

        // Check if file exists before trying to load
        if (!System.IO.File.Exists(fullPath))
        {
            Console.WriteLine($"Error: Sprite file not found at {fullPath}");
            // Return array filled with -1 (invalid handle)
            for (int i = 0; i < sprites.Length; ++i) sprites[i] = -1;
            return sprites;
        }

        int result = DX.LoadDivGraph(fullPath, spriteCount, divX, divY, sizeX, sizeY, sprites);

        if (result == -1)
        {
            Console.WriteLine($"Error: Failed to load/divide sprite: {fullPath}");
            // Fill with -1 on failure
            for (int i = 0; i < sprites.Length; ++i) sprites[i] = -1;
        }
        return sprites;
    }

    // DrawEXGraph is no longer needed as drawing is handled within Player/Ground
    /*
    public static void DrawEXGraph(int posX, int posY, int sizeX, int sizeY, int GrHandle) {
        // No camera offset needed here
        if (GrHandle >= 0) // Draw only if handle is valid
        {
             DX.DrawExtendGraph(posX, posY, posX + sizeX, posY + sizeY, GrHandle, DX.TRUE);
        }
    }
    */
}
