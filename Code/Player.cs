using System.Numerics;

using static Constants;

internal class Player {
    private int _imageIndex;
    private int _imageIndexCount;
    private bool _isFacingRight;

    private Vector2 _position;
    private Vector2 _movement;

    private Vector2 ImagePos => new Vector2(_position.X - (PLAYER_SIZE.X / 2), _position.Y - (PLAYER_SIZE.Y / 2));

    public Player() {
        _imageIndex = 0;
        _imageIndexCount = 0;
        _isFacingRight = true;

        _position = new Vector2(100f, 100f);
        _movement = Vector2.Zero;
    }

    public void Update(InputState input) {
        HandleInput(input);
        ApplyMovement();
    }

    private void HandleInput(InputState input) {
        _movement = Vector2.Zero;

        if (input.Up) Move(0, -1);
        if (input.Down) Move(0, 1);
        if (input.Left) Move(-1, 0, false);
        if (input.Right) Move(1, 0, true);
    }

    private void Move(int xDir, int yDir, bool? facingDirection = null) {
        if (xDir != 0) _movement.X += xDir * PLAYER_SPEED;
        if (yDir != 0) _movement.Y += yDir * PLAYER_SPEED;

        if (facingDirection.HasValue) {
            _isFacingRight = facingDirection.Value;
        }
    }

    private void ApplyMovement() {
        _position += _movement;
    }

    public void Render() {
        UpdateImageIndex();

        Program.DrawEXGraph((int)ImagePos.X, (int)ImagePos.Y, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y, PLAYER_IMAGES[_imageIndex]);
    }

    private void UpdateImageIndex() {
        if (_movement.X != 0 || _movement.Y != 0) {
            _imageIndex = _isFacingRight ? 1 : 4;
            _imageIndexCount++;
            if (_imageIndexCount >= 10) {
                _imageIndex = _isFacingRight ? 2 : 5;
                if (_imageIndexCount >= 20) _imageIndexCount = 0;
            }
        } else {
            _imageIndex = _isFacingRight ? 0 : 3;
        }
    }
}
