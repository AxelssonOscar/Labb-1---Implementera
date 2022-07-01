using Raylib_cs;

namespace PongLecture.Game
{
    //Klassen Ball ärver interfacet IObserver för att spelet ska kunna pausa.
    public class Ball : IObserver
    {

        public float x, y;
        public float speedX, speedY;
        public float radius;

        public bool IsPaused { get; set; } = false;

        public Ball(float x, float y, float speedX, float speedY, float radius)
        {
            this.x = x;
            this.y = y;
            this.speedX = speedX;
            this.speedY = speedY;
            this.radius = radius;
        }
        public void Draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, Color.WHITE);
        }

        public void UpdatePosition()
        {
            if (!IsPaused)
            {
                x += speedX * Raylib.GetFrameTime();
                y += speedY * Raylib.GetFrameTime();

                if (y < 0)
                {
                    y = 0;
                    speedY *= -1;
                }

                if (y > Raylib.GetScreenHeight())
                {
                    y = Raylib.GetScreenHeight();
                    speedY *= -1;
                }
            }
        }

        public void Pause(bool pause)
        {
            IsPaused = pause;
        }

    }
}
