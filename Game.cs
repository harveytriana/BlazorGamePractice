// GAME MANAGER
// Blazor adaptation by: Harvey Triana
// 
using BlazorPong.Models;
using System;
using System.Threading.Tasks;

namespace BlazorPong
{
    public class Game
    {
        // Could be a config object. In this sample, keeps simple hardcode
        public const int
            BALLRATIO = 10,
            BALLDIAMETER = 2 * BALLRATIO,
            WIDTH = 700,
            HEIGHT = 500,
            BARHEIGHT = 100,
            BARWIDTH = 20,
            YMAX = HEIGHT - BALLDIAMETER,
            YMIN = BALLDIAMETER;

        // main react event
        public EventHandler Loop;

        // messages to container
        public delegate void PromptHandler(string message);
        public PromptHandler Prompt;

        // gamme objects
        public BallModel Ball { get; private set; }
        public BarModel Bar1 { get; private set; }
        public BarModel Bar2 { get; private set; }

        // gamme control
        public bool OnGame { get; set; }
        public int Speed { get; set; } = 50;
        public int BallMovement { get; set; } = 10;
        public int BarMovement { get; set; } = 20;


        public Game()
        {
            Ball = new BallModel();
            Bar1 = new BarModel { PlayerName = "Player1" };
            Bar2 = new BarModel { PlayerName = "Player2" };
            Reset();
        }

        public void Reset()
        {
            Bar1.Left = 0;
            Bar1.Top = 0;
            Bar2.Left = WIDTH - BARWIDTH;
            Bar2.Top = HEIGHT - BARHEIGHT;
            Ball.X = WIDTH / 2;
            Ball.Y = HEIGHT / 2;
            Ball.Direction = 1;
            Ball.State = 1;
        }

        public void StartGame()
        {
            if (OnGame == false) {
                OnGame = true;
                Reset();
                Play();
            }
        }

        public async void Play()
        {
            while (OnGame) {
                MoveBall();
                MoveBar();
                CheckLost();
                // react
                Loop.Invoke(this, EventArgs.Empty);
                // loop
                await Task.Delay(Speed);
            }
        }

        private void CheckLost()
        {
            if (Ball.X >= WIDTH) {
                GameOver();
                Bar1.Points++;
                // Console.WriteLine("Point Player 1");
            }
            if (Ball.X <= 0) {
                GameOver();
                Bar2.Points++;
                // Console.WriteLine("Point Player 2");
            }
            if (!OnGame) {
                Prompt.Invoke($"Gamme Over! Score: {Bar1.PlayerName}: {Bar1.Points} | {Bar2.PlayerName}: {Bar2.Points}");
            }
        }

        void MoveBar()
        {
            if (Bar1.KeyPressed) {
                if (Bar1.KeyCode == "KeyQ" && Bar1.Top > 0) {
                    Bar1.Top -= BarMovement;
                }
                if (Bar1.KeyCode == "KeyA" && Bar1.Top < HEIGHT - BARHEIGHT) {
                    Bar1.Top += BarMovement;
                }
            }
            if (Bar2.KeyPressed) {
                Console.WriteLine($"Bar2.Top: {Bar2.Top}");
                Console.WriteLine($"Bar2.Top-x: {Bar2.Top - BarMovement}");

                if (Bar2.KeyCode == "KeyO" && Bar2.Top > 0) {
                    Bar2.Top -= BarMovement;
                }
                if (Bar2.KeyCode == "KeyL" && Bar2.Top < HEIGHT - BARHEIGHT) {
                    Bar2.Top += BarMovement;
                }
            }
        }

        void MoveBall()
        {
            CheckBallState();
            switch (Ball.State) {
                case 1: // R-D
                    Ball.X += BallMovement;
                    Ball.Y += BallMovement;
                    break;
                case 2: // R-U
                    Ball.X += BallMovement;
                    Ball.Y -= BallMovement;
                    break;
                case 3: // L-D
                    Ball.X -= BallMovement;
                    Ball.Y += BallMovement;
                    break;
                case 4: // L-D
                    Ball.X -= BallMovement;
                    Ball.Y -= BallMovement;
                    break;
            }
        }

        void CheckBallState()
        {
            if (CollidePlayer2()) {
                Ball.Direction = 2;
                if (Ball.State == 1) Ball.State = 3;
                if (Ball.State == 2) Ball.State = 4;
            }
            else if (CollidePlayer1()) {
                Ball.Direction = 1;
                if (Ball.State == 3) Ball.State = 1;
                if (Ball.State == 4) Ball.State = 2;
            }
            if (Ball.Direction == 1) {
                if (Ball.Y >= YMAX) Ball.State = 2;
                else if (Ball.Y <= YMIN) Ball.State = 1;
            }
            else {// direction 2
                if (Ball.Y >= YMAX) Ball.State = 4;
                else if (Ball.Y <= YMIN) Ball.State = 3;
            }
        }

        private bool CollidePlayer1()
        {
            if (Ball.X - BALLRATIO <= BARWIDTH &&
                Ball.Y + BALLRATIO >= Bar1.Top &&
                Ball.Y + BALLRATIO <= (Bar1.Top + BARHEIGHT)) {
                return true;
            }
            return false;
        }

        private bool CollidePlayer2()
        {
            if (Ball.X + BALLRATIO >= WIDTH - BARWIDTH &&
                Ball.Y + BALLRATIO >= Bar2.Top &&
                Ball.Y + BALLRATIO <= (Bar2.Top + BARHEIGHT)) {
                return true;
            }
            return false;
        }

        public void GameOver()
        {
            OnGame = false;
        }
    }
}
