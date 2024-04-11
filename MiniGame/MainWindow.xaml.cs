using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MiniGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ball ballObject;
        uint leftScore = 0;
        uint rightScore = 0;
        static public bool notClosed = false;
        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        protected void StartGame()
        {
            notClosed = true;
            unsafe
            {
                this.ballObject = new Ball(this.Width / 2, this.Height / 2, random.Next((int)4)*Math.PI/2+Math.PI/6, new Rect(0,0,this.Width,this.Height),20,20);
                ballObject.MoveEvent += new EventHandler(BallMove);
            }
            this.ball.Margin=new Thickness(this.Width / 2, this.Height / 2,0,0);
            this.ballObject.CheckCollisionEvent += new EventHandler(Collision);
            this.ballObject.GotScore += new EventHandler(ScoreEventCathcer);



            Thread thread = new Thread(() =>
            {
                while(MainWindow.notClosed)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (Math.Sin(ballObject.angle) < 0)
                        {
                            if (rightPad.Margin.Top > 30)
                                this.rightPad.Margin = new Thickness(this.rightPad.Margin.Left, this.rightPad.Margin.Top - 15, this.rightPad.Margin.Right, this.rightPad.Margin.Bottom);
                        }
                        else
                        {
                            if (rightPad.Margin.Top + 150 + 15 * 4 < this.Height)
                                this.rightPad.Margin = new Thickness(this.rightPad.Margin.Left, this.rightPad.Margin.Top + 15, this.rightPad.Margin.Right, this.rightPad.Margin.Bottom);
                        }
                    });
                    Thread.Sleep(250);
                }
                
            });
            thread.Start();
        }





        private void BallMove(object sender, EventArgs eventArgs)
        {
            Dispatcher.Invoke(() => { this.ball.Margin = new Thickness(((CoordinatesEventArgs)(eventArgs)).x, ((CoordinatesEventArgs)(eventArgs)).y, 0, 0);});
        }


        private void ScoreEventCathcer(object sender, EventArgs eventArgs)
        {
            CoordinatesEventArgs coordinatesEventArgs=(CoordinatesEventArgs)eventArgs;
            if (coordinatesEventArgs.x <= 0)
                ++rightScore;
            else
                ++leftScore;

            Dispatcher.Invoke(() =>
            {
                this.leftScoreTextBlock.Text = leftScore.ToString();
                this.rightScoreTextBlock.Text = rightScore.ToString(); 
            });
            
        }
        private void Collision(object sender, EventArgs eventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                if (ballObject.GetX() < this.Width/2)
                    this.ballObject.CheckCollision(new Rect(this.lefPad.Margin.Left, this.lefPad.Margin.Top, this.lefPad.Width, this.lefPad.Height));
                else
                    this.ballObject.CheckCollision(new Rect(this.rightPad.Margin.Left, this.rightPad.Margin.Top, this.rightPad.Width, this.rightPad.Height));
            });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();


            if(e.Key == Key.Up || e.Key==Key.W)
            {
                if(lefPad.Margin.Top> 30)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top - 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
                if (lefPad.Margin.Top < 30)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, 30, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
            }
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                if (lefPad.Margin.Top + 150 + 15 * 3 < this.Height)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top + 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
                if (lefPad.Margin.Top+150+15*3 > this.Height)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top - 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.notClosed = false;
        }
    }
}
