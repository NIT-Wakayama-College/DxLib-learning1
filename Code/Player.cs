using System;
using System.Numerics;

using static Constants;

internal class Player {
    private int _gravity;
    private int _imageIndex;
    private int _imageIndexCount;
    private bool _isJumping;
    private bool _isFacingRight;

    private Vector2 _position;
    private Vector2 _movement;

    private Vector2 ImagePos1 => new Vector2(_position.X - (PLAYER_SIZE_X / 2), _position.Y - (PLAYER_SIZE_Y / 2));
    private Vector2 ImagePos2 => new Vector2(_position.X + (PLAYER_SIZE_X / 2), _position.Y + (PLAYER_SIZE_Y / 2));

    private Vector2 HitboxPos1 => new Vector2(ImagePos1.X + 1f, ImagePos1.Y + 20f);
    private Vector2 HitboxPos2 => new Vector2(ImagePos2.X - 1f, ImagePos2.Y - 1f);

    public Player() {
        _gravity = 0;
        _imageIndex = 0;
        _imageIndexCount = 0;
        _isJumping = false;
        _isFacingRight = true;

        _position = new Vector2(100f, 100f);
        _movement = new Vector2(0f, 0f);
    }

    #region Update

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
        ApplyMovement();
    }

    private void HandleInput(InputState input) {
        _movement.X = 0;

        if (input.Left) {
            MoveLeft();
        } else if (input.Right) {
            MoveRight();
        }

        UpdateImageIndex();

        if (input.Jump && !_isJumping) {
            Jump();
        }
    }

    private void MoveLeft() {
        _isFacingRight = false;
        _movement.X -= PLAYER_SPEED;
    }

    private void MoveRight() {
        _isFacingRight = true;
        _movement.X += PLAYER_SPEED;
    }

    private void UpdateImageIndex() {
        if (_movement.X != 0) {
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

    private void Jump() {
        _isJumping = true;
        _gravity = JUMP_POWER;
    }

    private void ApplyGravity() {
        _gravity += GRAVITY_INCREMENT;
        _movement.Y = _gravity;
    }

    private void ApplyMovement() {
        Vector2 newPos = new Vector2 {
            X = CheckCollisionHorizontal(),
            Y = CheckCollisionVertical()
        };

        _position = newPos;

        if (_position.X >= SCREEN_WIDTH / 2 && _movement.X > 0) {
            _position.X = SCREEN_WIDTH / 2;
            Game.CameraOffsetX += (int)_movement.X;
        }
    }

    private float CheckCollisionHorizontal() {
        float left = HitboxPos1.X + _movement.X;
        float right = HitboxPos2.X + _movement.X;

        float top = HitboxPos1.Y;
        float bottom = HitboxPos2.Y;

        if (_movement.X < 0) {
            if (IsWall(new Vector2(left, top)) || IsWall(new Vector2(left, bottom)))
                return (float)(Math.Ceiling(left / CHIP_SIZE) * CHIP_SIZE - (HitboxPos1.X - _position.X) - Game.CameraOffsetX % CHIP_SIZE + 1);
        } else if (_movement.X > 0) {
            if (IsWall(new Vector2(right, top)) || IsWall(new Vector2(right, bottom)))
                return (float)(Math.Ceiling(right / CHIP_SIZE) * CHIP_SIZE - (HitboxPos2.X - _position.X) - Game.CameraOffsetX % CHIP_SIZE - 1);
        }
        return _position.X + _movement.X;
    }

    private float CheckCollisionVertical() {
        float left = HitboxPos1.X;
        float right = HitboxPos2.X;

        float top = HitboxPos1.Y + _movement.Y;
        float bottom = HitboxPos2.Y + _movement.Y;

        if (_movement.Y < 0) {
            if (IsWall(new Vector2(left, top)) || IsWall(new Vector2(right, top))) {
                _gravity = 0;
                return (float)(Math.Ceiling(top / CHIP_SIZE) * CHIP_SIZE - (HitboxPos1.Y - _position.Y) + 1);
            }
        } else if (_movement.Y > 0) {
            if (IsWall(new Vector2(left, bottom)) || IsWall(new Vector2(right, bottom))) {
                _gravity = 0;
                _isJumping = false;
                return (float)(Math.Floor(bottom / CHIP_SIZE) * CHIP_SIZE - (HitboxPos2.Y - _position.Y) - 1);
            }
        }
        return _position.Y + _movement.Y;
    }

    private bool IsWall(Vector2 pos) => Ground.IsWall(pos);

    #endregion Update

    public void Render() {
        Program.DrawEXGraph((int)ImagePos1.X, (int)ImagePos1.Y, PLAYER_SIZE_X, PLAYER_SIZE_Y, PLAYER_IMAGES[_imageIndex]);
    }
}
