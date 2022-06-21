using System;
using System.Collections.Generic;
using Raylib_cs;

namespace PongLecture.Game
{
    public class Pong : ISubject
    {
        private static Pong instance = null;
        private static readonly object padlock = new object();

        public static Pong Instance()
        {
            lock (padlock)
            {
                if (instance == null)
                    instance = new Pong();

                return instance;
            }
        }

        private EasyPaddle easySkill;
        private MediumPaddle mediumPaddle;
        private HardPaddle hardSkill;

        private Paddle leftPaddle;
        private Paddle rightPaddle;

        private Ball ball;

        private int LeftPlayerScore = 0;
        private int RightPlayerScore = 0;

        private bool IsPaused { get; set; } = false;

        private Pong()
        {
            easySkill = new EasyPaddle();
            hardSkill = new HardPaddle();
            mediumPaddle = new MediumPaddle();

            leftPaddle = new Paddle(50, Raylib.GetScreenHeight() / 2, 500, 10, 100, mediumPaddle);
            rightPaddle = new Paddle(Raylib.GetScreenWidth() - 50, Raylib.GetScreenHeight() / 2, 500, 10, 100, mediumPaddle);

            ball = new Ball(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2, -300, 0, 5);

            Attach(leftPaddle);
            Attach(rightPaddle);
            Attach(ball);
        }


        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            _observers.Add(observer);
        }
        public void Detach(IObserver observer)
        {
            Console.WriteLine("Subject: Detached an observer.");
            _observers.Remove(observer);
        }

        public void Pause(bool pause)
        {
            Console.WriteLine("Pausing all observers");
            foreach (var observer in _observers)
            {
                observer.Pause(pause);
            }
        }


        public void GameLoop()
        {
            bool quit = false;
            bool gameWon = false;

            while (!quit)
            {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_ESCAPE) || Raylib.WindowShouldClose())
                    quit = true;

                if (Raylib.IsKeyReleased(KeyboardKey.KEY_SPACE))
                {
                    IsPaused = !IsPaused;
                    Pause(IsPaused);
                }

                if(Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && gameWon)
                {
                    gameWon = false;
                    ResetGame();
                }

                Update();
                CheckLeftCollision(ball, leftPaddle);
                CheckRightCollision(ball, rightPaddle);


                if(CheckScore(ball))
                {
                    AdjustDifficulty();
                    ResetBall();
                }

                int win = CheckWin();

                if(win > 0)
                {
                    switch(win)
                    {
                        case 1:
                            int lefttextwidth = Raylib.MeasureText("Left player won!", 80);
                            Raylib.DrawText("Left player won!", Raylib.GetScreenWidth() / 2 - lefttextwidth / 2, Raylib.GetScreenHeight() / 2 - 80, 80, Color.RED);
                            gameWon = true;
                            break;

                        case 2:
                            int righttextwidth = Raylib.MeasureText("Right player won!", 80);
                            Raylib.DrawText("Right player won!", Raylib.GetScreenWidth() / 2 - righttextwidth / 2, Raylib.GetScreenHeight() / 2 - 80, 80, Color.RED);
                            gameWon = true;
                            break;

                        default:
                            break;

                    }
                }

                if (IsPaused)
                {
                    int textwidth = Raylib.MeasureText("Game is paused", 60);
                    Raylib.DrawText("Game is paused", Raylib.GetScreenWidth() / 2 - textwidth / 2, 30, 60, Color.RED);
                }

                if(gameWon)
                {
                    int textwidth = Raylib.MeasureText("Press enter to start", 60);
                    ball.IsPaused = true;
                    Raylib.DrawText("Press enter to start", Raylib.GetScreenWidth() / 2 - textwidth / 2, Raylib.GetScreenHeight() / 2 + 80, 60, Color.RED);
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                Raylib.DrawText(Raylib.GetFPS().ToString(), 10, 10, 25, Color.WHITE);
                PrintScore();

                ball.Draw();
                leftPaddle.Draw();
                rightPaddle.Draw();

                Raylib.EndDrawing();
            }
        }

        public void Update()
        {
            ball.UpdatePosition();
            leftPaddle.UpdatePosition(1);
            //rightPaddle.UpdatePosition(2);
            rightPaddle.AI(ball.y);
        }

        private void PrintScore()
        {
            Raylib.DrawText("Left Player Score: " + LeftPlayerScore, 10, 40, 20, Color.BLUE);
            Raylib.DrawText("Right Player Score: " + RightPlayerScore, Raylib.GetScreenWidth() - 240, 40, 20, Color.BLUE);
        }

        private void AdjustDifficulty()
        {
            int scoreDifference = LeftPlayerScore - RightPlayerScore;

            if(scoreDifference > 5)
            {
                leftPaddle.UpdatePlayerSkill(hardSkill);
                rightPaddle.UpdatePlayerSkill(easySkill);
            }
            else if(scoreDifference > 2)
            {
                leftPaddle.UpdatePlayerSkill(hardSkill);
            }
            else if(scoreDifference < -2)
            {
                rightPaddle.UpdatePlayerSkill(hardSkill);
            }
            else if(scoreDifference < -5)
            {
                leftPaddle.UpdatePlayerSkill(easySkill);
                rightPaddle.UpdatePlayerSkill(hardSkill);
            }
            else
            {
                leftPaddle.UpdatePlayerSkill(mediumPaddle);
                rightPaddle.UpdatePlayerSkill(mediumPaddle);
            }

        }


        private bool CheckScore(Ball ball)
        {
            if (ball.x < 0)
            {
                RightPlayerScore++;
                return true;
            }
            if (ball.x > Raylib.GetScreenWidth())
            {
                LeftPlayerScore++;
                return true;
            }
            return false;
        }

        private int CheckWin()
        {
            if(LeftPlayerScore >= 10)
            {
                return 1;
            }

            if(RightPlayerScore >= 10)
            {
                return 2;
            }

            return 0;
        }

        private void ResetGame()
        {
            LeftPlayerScore = 0;
            RightPlayerScore = 0;
            leftPaddle.UpdatePlayerSkill(mediumPaddle);
            rightPaddle.UpdatePlayerSkill(mediumPaddle);
            ResetBall();
        }

        private void ResetBall()
        {
            ball.IsPaused = false;
            ball.x = Raylib.GetScreenWidth() / 2;
            ball.y = Raylib.GetScreenHeight() / 2;
            ball.speedX = 300;
            ball.speedY = 300;
        }


        private void CheckLeftCollision(Ball ball, Paddle paddle)
        {
            System.Numerics.Vector2 ballVector = new System.Numerics.Vector2(ball.x, ball.y);
            if (Raylib.CheckCollisionCircleRec(ballVector, ball.radius, paddle.GetPaddle()))
            {
                ball.speedX *= -1.1f;
                ball.speedY = (ball.y - paddle.y) / (paddle.height / 2) * ball.speedX;
            }
        }

        private void CheckRightCollision(Ball ball, Paddle paddle)
        {
            System.Numerics.Vector2 ballVector = new System.Numerics.Vector2(ball.x, ball.y);
            if (Raylib.CheckCollisionCircleRec(ballVector, ball.radius, paddle.GetPaddle()))
            {
                ball.speedX *= -1.1f;
                ball.speedY = (ball.y - paddle.y) / (paddle.height / 2) * -ball.speedX;
            }
        }

    }
}
