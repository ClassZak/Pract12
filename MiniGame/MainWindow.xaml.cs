using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
            Dispatcher.Invoke(() => { });
        }

        protected void StartGame()
        {
            unsafe
            {
                this.ballObject = new Ball(this.Width / 2, this.Height / 2, random.Next((int)(Math.PI * 100) + 1) / 100.0, new Rect(0,0,this.Width,this.Height),20,20);
                ballObject.MoveEvent += new EventHandler(BallMove);
            }
            this.ball.Margin=new Thickness(this.Width / 2, this.Height / 2,0,0);
            this.ballObject.CheckCollisionEvent += new EventHandler(Collision);
        }





        private void BallMove(object sender, EventArgs eventArgs)
        {
            Dispatcher.Invoke(() => { this.ball.Margin = new Thickness(((CoordinatesEventArgs)(eventArgs)).x, ((CoordinatesEventArgs)(eventArgs)).y, 0, 0);});
        }
        private void Collision(object sender, EventArgs eventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                if (ballObject.GetX() < 40)
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
                if(lefPad.Margin.Top> 15)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top - 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
                if (lefPad.Margin.Top < 0)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, 0, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
            }
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                if (lefPad.Margin.Top + 150 + 15 * 4 < this.Height)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top + 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
                if (lefPad.Margin.Top+150+15*4 > this.Height)
                    this.lefPad.Margin = new Thickness(this.lefPad.Margin.Left, this.lefPad.Margin.Top - 15, this.lefPad.Margin.Right, this.lefPad.Margin.Bottom);
            }
        }
    }
}
