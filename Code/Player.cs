using System.Numerics;

using static Constants;

class Player {
    int _gravity = 0;
    bool _isJumping = false;

    Vector2 _position = new Vector2(100f, 100f);
    Vector2 _movement = new Vector2(0f, 0f);

    public Vector2 ImagePos1 => new Vector2(_position.X - (PLAYER_SIZE / 2), _position.Y - (PLAYER_SIZE / 2));
    public Vector2 ImagePos2 => new Vector2(_position.X + (PLAYER_SIZE / 2), _position.Y + (PLAYER_SIZE / 2));

    public Vector2 HitboxPos1 => new Vector2(ImagePos1.X + 1f, ImagePos1.Y + 1f);
    public Vector2 HitboxPos2 => new Vector2(ImagePos2.X - 1f, ImagePos2.Y - 1f);

    #region Update

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
        ApplyMovement();
    }

    void HandleInput(InputState input) {
        _movement.X = 0;

        if (input.Left) _movement.X -= PLAYER_SPEED;
        if (input.Right) _movement.X += PLAYER_SPEED;

        if (input.Jump && !_isJumping) {
            _isJumping = true;
            _gravity = JUMP_POWER;
        }
    }

    void ApplyGravity() {
        _gravity += GRAVITY_INCREMENT;
        _movement.Y = _gravity;
    }

    void ApplyMovement() {
        Vector2 newPos = new Vector2 {
            X = Ground.CheckCollisionHorizontal(_position, _movement.X, PLAYER_SIZE),
            Y = Ground.CheckCollisionVertical(_position, _movement.Y, PLAYER_SIZE, ref _gravity, ref _isJumping)
        };

        _position = newPos;
    }

    #endregion Update

    public void Render() {
        Program.DrawBox((int)ImagePos1.X, (int)ImagePos1.Y, PLAYER_SIZE, PLAYER_SIZE, COLOR_WHITE);
    }
}
