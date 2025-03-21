using System;
using System.Numerics;

using static Constants;

internal class Player {
    public static int CameraOffsetX { get; private set; }

    private int _gravity;
    private int _imageIndex;
    private int _imageIndexCount;
    private bool _isJumping;
    private bool _isFacingRight;

    private Vector2 _position;
    private Vector2 _movement;

    private Vector2 ImagePos1 => new Vector2(_position.X - (PLAYER_SIZE.X / 2), _position.Y - (PLAYER_SIZE.Y / 2));
    private Vector2 ImagePos2 => new Vector2(_position.X + (PLAYER_SIZE.X / 2), _position.Y + (PLAYER_SIZE.Y / 2));

    private Vector2 HitboxPos1 => new Vector2(ImagePos1.X, ImagePos1.Y + 20f);
    private Vector2 HitboxPos2 => new Vector2(ImagePos2.X, ImagePos2.Y);

    public Player() {
        _gravity = 0;
        _imageIndex = 0;
        _imageIndexCount = 0;
        _isJumping = false;
        _isFacingRight = true;

        _position = new Vector2(100f, 100f);
        _movement = Vector2.Zero;
    }

    public void Update(InputState input) {
        HandleInput(input);
        ApplyGravity();
        ApplyMovement();
    }

    private void HandleInput(InputState input) {
        _movement = Vector2.Zero;

        if (input.Left) Move(-1, false);
        if (input.Right) Move(1, true);
        if (input.Jump && !_isJumping) Jump();
    }

    private void Move(int xDir, bool facingDirection) {
        _movement.X += xDir * PLAYER_SPEED;
        _isFacingRight = facingDirection;
    }

    private void Jump() {
        _isJumping = true;
        _gravity = JUMP_POWER;
    }

    private void ApplyGravity() {
        _gravity += GRAVITY_INCREMENT;
        _movement.Y += _gravity;
    }

    private void ApplyMovement() {
        _position = new Vector2 {
            X = CheckCollisionHorizontal(),
            Y = CheckCollisionVertical(),
        };

        if (_position.X >= SCREEN_SIZE.X / 2 && _movement.X > 0) {
            _position.X = SCREEN_SIZE.X / 2;
            CameraOffsetX += (int)_movement.X;
        }
    }

    private float CheckCollisionHorizontal() {
        float left = HitboxPos1.X + _movement.X + 1;
        float right = HitboxPos2.X + _movement.X - 1;

        float top = HitboxPos1.Y + 1;
        float bottom = HitboxPos2.Y - 1;

        if (_movement.X < 0) {
            if (IsWall(new Vector2(left, top)) || IsWall(new Vector2(left, bottom)))
                return (float)(Math.Ceiling(left / GROUND_SIZE) * GROUND_SIZE + (HitboxPos1.X - _position.X) - CameraOffsetX % GROUND_SIZE);
        } else if (_movement.X > 0) {
            if (IsWall(new Vector2(right, top)) || IsWall(new Vector2(right, bottom)))
                return (float)(Math.Floor(right / GROUND_SIZE) * GROUND_SIZE - (HitboxPos2.X - _position.X) - CameraOffsetX % GROUND_SIZE);
        }
        return _position.X + _movement.X;
    }


    private float CheckCollisionVertical() {
        float left = HitboxPos1.X + 1;
        float right = HitboxPos2.X - 1;

        float top = HitboxPos1.Y + _movement.Y + 1;
        float bottom = HitboxPos2.Y + _movement.Y - 1;

        if (_movement.Y < 0) {
            if (IsWall(new Vector2(left, top)) || IsWall(new Vector2(right, top))) {
                _gravity = 0;
                return (float)(Math.Ceiling(top / GROUND_SIZE) * GROUND_SIZE + (HitboxPos1.Y - _position.Y));
            }
        } else if (_movement.Y > 0) {
            if (IsWall(new Vector2(left, bottom)) || IsWall(new Vector2(right, bottom))) {
                _gravity = 0;
                _isJumping = false;
                return (float)(Math.Floor(bottom / GROUND_SIZE) * GROUND_SIZE - (HitboxPos2.Y - _position.Y));
            }
        }
        return _position.Y + _movement.Y;
    }

    private bool IsWall(Vector2 pos) => Ground.IsWall(pos);

    public void Render() {
        UpdateImageIndex();

        Program.DrawEXGraph((int)ImagePos1.X + CameraOffsetX, (int)ImagePos1.Y, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y, PLAYER_IMAGES[_imageIndex]);
    }

    private void UpdateImageIndex() {
        if (_movement.X != 0 || _isJumping) {
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
