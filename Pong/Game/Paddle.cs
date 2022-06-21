using Raylib_cs;

namespace PongLecture.Game
{
    public class Paddle : IObserver
    {
        public float x, y;
        public float speed;
        public float width, height;

        private bool IsPaused { get; set; } = false;

        public IPaddleSkill skill;

        public Paddle(float x, float y, float speed, float width, float height, IPaddleSkill skill)
        {
            this.x = x;
            this.y = y;
            this.speed = speed;
            this.width = width;

            this.skill = skill;
            this.height = this.skill.GetHeight(height);
        }

        public void UpdatePlayerSkill(IPaddleSkill skill)
        {
            this.skill = skill;
        }

        public Rectangle GetPaddle()
        {
            return new Rectangle(x - width / 2, y - height / 2, width, skill.GetHeight(height));
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(GetPaddle(), Color.WHITE);
        }

        public void UpdatePosition(int player)
        {
            if (!IsPaused)
            {
                if (player == 1)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                        y -= speed * Raylib.GetFrameTime();
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                        y += speed * Raylib.GetFrameTime();
                }
                else if (player == 2)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
                        y -= speed * Raylib.GetFrameTime();
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
                        y += speed * Raylib.GetFrameTime();
                }
            }
        }

        public void Pause(bool pause)
        {
            IsPaused = pause;
        }
    }
}
