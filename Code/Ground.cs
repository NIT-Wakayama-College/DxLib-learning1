using System;
using System.Numerics;

using static Constants;

static class Ground {

    public static void Render() {
        for (int i = 0; i < MAP_HEIGHT; i++) {
            for (int j = 0; j < MAP_WIDTH; j++) {
                if (MAP_DATA[i, j] == 0) {
                    Program.DrawBox(j * CHIP_SIZE, i * CHIP_SIZE, CHIP_SIZE, CHIP_SIZE, COLOR_RED);
                }
            }
        }
    }

    public static float CheckCollisionHorizontal(Vector2 pos, float dx, int size) {
        float left = pos.X - size / 2 + dx;
        float right = pos.X + size / 2 + dx;

        if (dx < 0) {
            if (IsWall(new Vector2(left, pos.Y)))
                return (float)(Math.Ceiling(left / CHIP_SIZE) * CHIP_SIZE + size / 2);
        } else if (dx > 0) {
            if (IsWall(new Vector2(right, pos.Y)))
                return (float)(Math.Floor(right / CHIP_SIZE) * CHIP_SIZE - size / 2);
        }
        return pos.X + dx;
    }

    public static float CheckCollisionVertical(Vector2 pos, float dy, int size, ref int _gravity, ref bool _isJumping) {
        float top = pos.Y - size / 2 + dy;
        float bottom = pos.Y + size / 2 + dy;

        if (dy < 0) {
            if (IsWall(new Vector2(pos.X, top)))
                return (float)(Math.Ceiling(top / CHIP_SIZE) * CHIP_SIZE + size / 2);
        } else if (dy > 0) {
            if (IsWall(new Vector2(pos.X, bottom))) {
                _gravity = 0;
                _isJumping = false;
                return (float)(Math.Floor(bottom / CHIP_SIZE) * CHIP_SIZE - size / 2);
            }
        }
        return pos.Y + dy;
    }

    public static bool IsWall(Vector2 pos) {
        return GetChipParam(pos) != 1;
    }

    public static int GetChipParam(Vector2 pos) {
        if (pos.X < 0f || pos.X >= SCREEN_WIDTH) return 0;
        if (pos.Y < 0f || pos.Y >= SCREEN_HEIGHT) return 0;

        int gridX = (int)(pos.X / CHIP_SIZE);
        int gridY = (int)(pos.Y / CHIP_SIZE);

        return MAP_DATA[gridY, gridX];
    }
}
