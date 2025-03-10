class Player {
    int _playerX = 0, _playerY = 0;
    const int SPEED = 3;
    const int SIZE = 32;

    public void Update(InputState input) {
        if (input.Up) _playerY -= SPEED;
        if (input.Down) _playerY += SPEED;
        if (input.Left) _playerX -= SPEED;
        if (input.Right) _playerX += SPEED;
    }

    public void Render() {
        Program.DrawBox(_playerX, _playerY, SIZE, SIZE, Program.COLOR_WHITE);
    }
}
