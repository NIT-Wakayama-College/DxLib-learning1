using System.Numerics;

class Player {
    const int SPEED = 3;
    const int SIZE = 32;
    const int JUMP_FORCE = -20;
    const int GRAVITY_INCREMENT = 1;

    int _gravity = 0;
    bool _isJumping = false;

    Vector2 _position = new Vector2(100f, 100f);
    Vector2 _movement = new Vector2(0f, 0f);

    public Vector2 ImagePos1 => new Vector2(_position.X - (SIZE / 2), _position.Y - (SIZE / 2));
    public Vector2 ImagePos2 => new Vector2(_position.X + (SIZE / 2), _position.Y + (SIZE / 2));

    public Vector2 HitboxPos1 => new Vector2(ImagePos1.X + 1f, ImagePos1.X + 1f);
    public Vector2 HitboxPos2 => new Vector2(ImagePos1.X - 1f, ImagePos1.X - 1f);

    #region Update

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
        ApplyMovement();
    }

    void HandleInput(InputState input) {
        _movement.X = 0;

        if (input.Left) _movement.X -= SPEED;
        if (input.Right) _movement.X += SPEED;

        if (input.Jump && !_isJumping) {
            _isJumping = true;
            _gravity = JUMP_FORCE;
        }
    }

    void ApplyGravity() {
        _gravity += GRAVITY_INCREMENT;
        _movement.Y = _gravity;
    }

    void ApplyMovement() {
        Vector2 newPos = new Vector2 {
            X = Ground.CheckCollisionHorizontal(_position, _movement.X, SIZE),
            Y = Ground.CheckCollisionVertical(_position, _movement.Y, SIZE, ref _gravity, ref _isJumping)
        };

        _position = newPos;
    }

    #endregion Update

    public void Render() {
        Program.DrawBox((int)ImagePos1.X, (int)ImagePos1.Y, SIZE, SIZE, Program.COLOR_WHITE);
    }
}
