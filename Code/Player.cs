using System.Numerics;
using DxLibDLL;
using System; // For Math
using static Constants; // Access constants like PLAYER_SPEED, GRAVITY_INCREMENT etc.

internal class Player
{
    // Player State
    private int _gravity; // Represents vertical velocity, affected by gravity
    private int _imageIndex;
    private int _imageIndexCount; // Timer for animation frames
    private bool _isJumping; // True if currently in the air (initiated by jump or falling)
    private bool _isFacingRight;

    // Position and Movement (World Coordinates)
    private Vector2 _position;
    private Vector2 _movement; // Calculated movement vector for the current frame

    // Public property to get player's world position (read-only from outside)
    public Vector2 Position => _position;
    // Optional: public property for debugging jump state
    // public bool IsJumping => _isJumping;

    /// <summary>
    /// Initializes the player at a specific world position.
    /// </summary>
    /// <param name="startPosition">The starting world coordinates.</param>
    public Player(Vector2 startPosition)
    {
        _position = startPosition;

        _gravity = 0;
        _imageIndex = 0;
        _imageIndexCount = 0;
        _isJumping = true; // Assume starting in the air until first ground collision
        _isFacingRight = true;
        _movement = Vector2.Zero;
    }

    /// <summary>
    /// Updates the player's state, including input handling, physics, and collision.
    /// </summary>
    /// <param name="input">The current input state.</param>
    public void Update(InputState input)
    {
        HandleInput(input);
        ApplyGravity();
        MoveAndCollide();
        UpdateAnimation(); // Update animation after movement/collision
    }

    /// <summary>
    /// Processes player input to determine intended movement.
    /// </summary>
    private void HandleInput(InputState input)
    {
        _movement.X = 0; // Reset horizontal movement intention each frame

        if (input.Left)
        {
            _movement.X -= PLAYER_SPEED;
            _isFacingRight = false;
        }
        if (input.Right)
        {
            _movement.X += PLAYER_SPEED;
            _isFacingRight = true;
        }
        // Allow jumping only if on the ground
        if (input.Jump && !_isJumping)
        {
            Jump();
        }
    }

    /// <summary>
    /// Initiates a jump by setting vertical velocity.
    /// </summary>
    private void Jump()
    {
        _isJumping = true;
        _gravity = JUMP_POWER; // Apply upward force (negative value)
        // Note: _movement.Y will be updated in ApplyGravity based on _gravity
    }

    /// <summary>
    /// Applies gravity effect to the vertical velocity (_gravity).
    /// </summary>
    private void ApplyGravity()
    {
        _gravity += GRAVITY_INCREMENT;
        // Update the vertical movement component based on the current gravity/velocity
        _movement.Y = _gravity;
    }

    /// <summary>
    /// Applies the calculated movement and resolves collisions with the map.
    /// </summary>
    private void MoveAndCollide()
    {
        // --- Horizontal Movement and Collision ---
        _position.X += _movement.X;
        ResolveCollisionHorizontal(); // Corrects _position.X if collision occurs

        // --- Vertical Movement and Collision ---
        _position.Y += _movement.Y;
        ResolveCollisionVertical();   // Corrects _position.Y and updates physics state (_gravity, _isJumping)
    }

    /// <summary>
    /// Detects and resolves horizontal collisions with solid tiles.
    /// Adjusts the player's X position if a collision occurs.
    /// </summary>
    private void ResolveCollisionHorizontal()
    {
        // Calculate player bounds based on the *potential* new position
        float halfWidth = PLAYER_HALF_SIZE.X;
        float halfHeight = PLAYER_HALF_SIZE.Y;

        // Determine the vertical range of tiles to check based on player height
        int topTile = (int)Math.Floor((_position.Y - halfHeight) / GROUND_SIZE);
        int bottomTile = (int)Math.Floor((_position.Y + halfHeight - 0.001f) / GROUND_SIZE); // Use small offset for edge cases

        // Clamp Y range to map bounds
        topTile = Math.Max(0, topTile);
        bottomTile = Math.Min(MAP_HEIGHT - 1, bottomTile);

        // Check Left Collision
        if (_movement.X < 0)
        {
            int leftTileX = (int)Math.Floor((_position.X - halfWidth) / GROUND_SIZE);
            for (int y = topTile; y <= bottomTile; y++)
            {
                if (Constants.IsSolidTile(leftTileX, y))
                {
                    // Collision detected: Snap player's left edge to the tile's right edge
                    _position.X = (leftTileX + 1) * GROUND_SIZE + halfWidth;
                    _movement.X = 0; // Stop horizontal movement
                    return; // Exit after resolving collision
                }
            }
        }
        // Check Right Collision
        else if (_movement.X > 0)
        {
            int rightTileX = (int)Math.Floor((_position.X + halfWidth - 0.001f) / GROUND_SIZE);
            for (int y = topTile; y <= bottomTile; y++)
            {
                if (Constants.IsSolidTile(rightTileX, y))
                {
                    // Collision detected: Snap player's right edge to the tile's left edge
                    _position.X = rightTileX * GROUND_SIZE - halfWidth;
                    _movement.X = 0; // Stop horizontal movement
                    return; // Exit after resolving collision
                }
            }
        }
    }

    /// <summary>
    /// Detects and resolves vertical collisions with solid tiles (ground/ceiling).
    /// Adjusts the player's Y position and updates physics state (_gravity, _isJumping).
    /// </summary>
    private void ResolveCollisionVertical()
    {
        float halfWidth = PLAYER_HALF_SIZE.X;
        float halfHeight = PLAYER_HALF_SIZE.Y;

        // Determine the horizontal range of tiles to check based on player width
        int leftTileX = (int)Math.Floor((_position.X - halfWidth) / GROUND_SIZE);
        int rightTileX = (int)Math.Floor((_position.X + halfWidth - 0.001f) / GROUND_SIZE);

        // Clamp X range to map bounds
        leftTileX = Math.Max(0, leftTileX);
        rightTileX = Math.Min(MAP_WIDTH - 1, rightTileX);

        // Check Downward Collision (Landing)
        if (_movement.Y > 0)
        { // Moving down
            int bottomTileY = (int)Math.Floor((_position.Y + halfHeight - 0.001f) / GROUND_SIZE);

            // Check for falling off the map
            if (bottomTileY >= MAP_HEIGHT)
            {
                // Handle falling off: respawn, game over, etc.
                Console.WriteLine("Player fell off the map!");
                // Simple respawn example:
                _position = new Vector2(100f, 100f); // Reset to start or checkpoint
                _gravity = 0;
                _isJumping = false;
                _movement = Vector2.Zero;
                return;
            }

            for (int x = leftTileX; x <= rightTileX; x++)
            {
                if (Constants.IsSolidTile(x, bottomTileY))
                {
                    // Collision detected: Snap player's bottom edge to the tile's top edge
                    _position.Y = bottomTileY * GROUND_SIZE - halfHeight;
                    _gravity = 0;       // Reset vertical velocity
                    _isJumping = false; // Player is now grounded
                    _movement.Y = 0;    // Stop vertical movement
                    return;             // Exit after resolving collision
                }
            }
            // If no ground collision detected while moving down, player is falling
            _isJumping = true;
        }
        // Check Upward Collision (Hitting Ceiling)
        else if (_movement.Y < 0)
        { // Moving up
            int topTileY = (int)Math.Floor((_position.Y - halfHeight) / GROUND_SIZE);

            // Check hitting top map boundary
            if (topTileY < 0)
            {
                _position.Y = halfHeight; // Stop at the top edge
                _gravity = 0; // Stop upward velocity
                _movement.Y = 0;
                return;
            }

            for (int x = leftTileX; x <= rightTileX; x++)
            {
                if (Constants.IsSolidTile(x, topTileY))
                {
                    // Collision detected: Snap player's top edge to the tile's bottom edge
                    _position.Y = (topTileY + 1) * GROUND_SIZE + halfHeight;
                    _gravity = 0;    // Reset vertical velocity (stop upward movement)
                    _movement.Y = 0; // Stop vertical movement
                    return;          // Exit after resolving collision
                }
            }
        }
    }

    /// <summary>
    /// Updates the animation frame (_imageIndex) based on player state.
    /// </summary>
    private void UpdateAnimation()
    {
        const int animSpeed = 10; // Frames per animation image change

        // Jumping Animation
        if (_isJumping)
        {
            _imageIndex = _isFacingRight ? 1 : 4; // Use frame 1 (right) or 4 (left) for jump
            _imageIndexCount = 0;
        }
        // Walking Animation
        else if (_movement.X != 0)
        {
            _imageIndexCount++;
            if (_imageIndexCount >= animSpeed * 2)
            { // Loop animation (2 frames)
                _imageIndexCount = 0;
            }

            if (_imageIndexCount < animSpeed)
            {
                _imageIndex = _isFacingRight ? 1 : 4; // Walking frame 1
            }
            else
            {
                _imageIndex = _isFacingRight ? 2 : 5; // Walking frame 2
            }
        }
        // Idle Animation
        else
        {
            _imageIndex = _isFacingRight ? 0 : 3; // Idle frame
            _imageIndexCount = 0;
        }
    }

    /// <summary>
    /// Renders the player sprite at the correct screen position.
    /// </summary>
    public void Render()
    {
        // Calculate screen position
        // Player is always centered horizontally
        float screenX = SCREEN_SIZE.X / 2f - PLAYER_HALF_SIZE.X;
        // Vertical position is based on world Y (no vertical camera scroll)
        float screenY = _position.Y - PLAYER_HALF_SIZE.Y;

        int imageHandle = -1;
        // Ensure image index is valid before accessing array
        if (_imageIndex >= 0 && _imageIndex < PLAYER_IMAGES.Length)
        {
            imageHandle = PLAYER_IMAGES[_imageIndex];
        }

        // Only draw if the image handle is valid
        if (imageHandle >= 0)
        {
            // Draw the player sprite using DxLib (Directly calling DX function)
            DX.DrawExtendGraph(
                (int)screenX,
                (int)screenY,
                (int)(screenX + PLAYER_SIZE.X),
                (int)(screenY + PLAYER_SIZE.Y),
                imageHandle,
                DX.TRUE // Enable transparency
            );
        }
        else
        {
            // Optional: Draw a placeholder if image is missing
            // DX.DrawBox((int)screenX, (int)screenY, (int)(screenX + PLAYER_SIZE.X), (int)(screenY + PLAYER_SIZE.Y), DX.GetColor(255,0,255), DX.TRUE);
        }

        // --- Debug Drawing (Optional) ---
        // Draw Hitbox Outline
        // uint hitboxColor = DX.GetColor(255, 0, 0); // Red
        // DX.DrawBox((int)screenX, (int)screenY, (int)(screenX + PLAYER_SIZE.X), (int)(screenY + PLAYER_SIZE.Y), hitboxColor, DX.FALSE); // FALSE for outline

        // Draw Position Marker (Center)
        // uint posColor = DX.GetColor(0, 255, 0); // Green
        // DX.DrawPixel((int)(screenX + PLAYER_HALF_SIZE.X), (int)(screenY + PLAYER_HALF_SIZE.Y), posColor);
    }
}
