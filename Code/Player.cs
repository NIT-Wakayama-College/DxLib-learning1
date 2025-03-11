class Player {
    const int SPEED = 3;
    const int SIZE = 32;
    const int JUMP_FORCE = -20;
    const int GRAVITY_INCREMENT = 1;

    int _gravity = 0;
    bool _isJumping = false;

    int _positionX = 100, _positionY = 100;
    int _movementX = 0, _movementY = 0;

    public int ImagePosX1 => _positionX - (SIZE / 2);
    public int ImagePosX2 => _positionX + (SIZE / 2);
    public int ImagePosY1 => _positionY - (SIZE / 2);
    public int ImagePosY2 => _positionY + (SIZE / 2);

    public int HitboxPosX1 => ImagePosX1 + 1;
    public int HitboxPosX2 => ImagePosX2 - 1;
    public int HitboxPosY1 => ImagePosY1 + 1;
    public int HitboxPosY2 => ImagePosY2 - 1;

    #region Update

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
        ApplyMovement();
    }

    void HandleInput(InputState input) {
        _movementX = 0;
        _movementY = 0;

        if (input.Left) _movementX -= SPEED;
        if (input.Right) _movementX += SPEED;

        if (input.Jump && !_isJumping) {
            _isJumping = true;
            _gravity = JUMP_FORCE;
        }
    }

    void ApplyGravity() {
        _gravity += GRAVITY_INCREMENT;
        _movementY += _gravity;
    }

    void ApplyMovement() {
        int newPosX = Ground.CheckCollisionHorizontal(_positionX, _positionY, _movementX, SIZE);
        int newPosY = Ground.CheckCollisionVertical(_positionX, _positionY, _movementY, SIZE, ref _gravity, ref _isJumping);

        _positionX = newPosX;
        _positionY = newPosY;
    }

    #endregion Update

    public void Render() {
        Program.DrawBox(ImagePosX1, ImagePosY1, SIZE, SIZE, Program.COLOR_WHITE);
    }
}
