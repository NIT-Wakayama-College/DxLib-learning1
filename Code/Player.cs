class Player {
    int PlayerX = 0, PlayerY = 0;
    const int SPEED = 3;
    const int SIZE = 32;

    public void Update(InputState input) {
        if (input.Up) PlayerY -= SPEED;
        if (input.Down) PlayerY += SPEED;
        if (input.Left) PlayerX -= SPEED;
        if (input.Right) PlayerX += SPEED;
    }

    public void Render() {
        Program.DrawBox(PlayerX, PlayerY, SIZE, SIZE, Program.COLOR_WHITE);
    }
}
