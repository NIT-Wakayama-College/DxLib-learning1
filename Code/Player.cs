using System.Numerics;

using static Constants;

internal class Player {
    private Vector2 _position;

    private Vector2 ImagePos => new Vector2(_position.X - (PLAYER_SIZE.X / 2), _position.Y - (PLAYER_SIZE.Y / 2));

    public Player() {
        _position = new Vector2(100f, 100f);
    }

    public void Render() {
        Program.DrawEXGraph((int)ImagePos.X, (int)ImagePos.Y, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y, PLAYER_IMAGES[0]);
    }
}
