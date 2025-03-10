class Player {
    int _playerX = 0, _playerY = 0;
    int _gravity = 0;

    const int SPEED = 3;
    const int SIZE = 32;
    const int JUMP_FORCE = -20;
    const int GRAVITY_INCREMENT = 1;

    #region Update

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
    }

    void HandleInput(InputState input) {
        if (input.Left) _playerX -= SPEED;
        if (input.Right) _playerX += SPEED;

        if (input.Jump && _playerY == 300) _gravity = JUMP_FORCE;
    }

    void ApplyGravity() {
        _playerY += _gravity;
        _gravity += GRAVITY_INCREMENT;

        if (_playerY > Game.GROUND_Y) {
            _playerY = Game.GROUND_Y;
            _gravity = 0;
        }
    }

    #endregion Update

    public void Render() {
        Program.DrawBox(_playerX, _playerY, SIZE, SIZE, Program.COLOR_WHITE);
    }
}
