using DxLibDLL;
using System.Numerics; // For Vector2

internal struct InputState
{
    public bool Left;
    public bool Right;
    public bool Jump;
    // Add other inputs like Attack, Dash etc. if needed
}

internal class Game
{
    private Player Player;
    private InputState InputState;

    public Game()
    {
        // Initialize Player with a starting position (e.g., read from map data or constants)
        Player = new Player(new Vector2(100f, 100f));
    }

    public void Run()
    {
        // Main game loop
        // Exit loop if ProcessMessage returns non-zero (e.g., window closed)
        // or if ESC key is pressed
        while (DX.ProcessMessage() == 0 && DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 0)
        {
            Update();
            Render();
        }
    }

    private void Update()
    {
        // Get current input state
        InputState = GetCurrentInput();
        // Update the player based on input
        Player.Update(InputState);
        // Update other game elements (enemies, items, etc.) here
    }

    private InputState GetCurrentInput()
    {
        // Check DxLib key states
        // Using CheckHitKey checks if the key is currently pressed down
        return new InputState
        {
            Left = DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1,
            Right = DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1,
            Jump = DX.CheckHitKey(DX.KEY_INPUT_SPACE) == 1,
            // For actions like jumping, you might want to check for the key press *edge*
            // (pressed this frame but not last frame) to prevent continuous jumping.
            // This requires storing the previous frame's input state.
        };
    }

    private void Render()
    {
        // Clear the back buffer
        DX.ClearDrawScreen();

        // Render the scrolling background/tiles based on player position
        Ground.Render(Player.Position);

        // Render the player (handles its own screen positioning)
        Player.Render();

        // Render UI, HUD, debug info etc. here
        // Example Debug Info:
        // DX.DrawString(5, 5, $"Pos: {Player.Position.X:F1}, {Player.Position.Y:F1}", DX.GetColor(255, 255, 255));
        // DX.DrawString(5, 25, $"Jumping: {Player.IsJumping}", DX.GetColor(255, 255, 255)); // Requires IsJumping property

        // Swap the back buffer to the screen
        DX.ScreenFlip();
    }
}