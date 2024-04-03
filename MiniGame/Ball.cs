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
using System.IO;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Threading;

namespace MiniGame
{
    class CoordinatesEventArgs : EventArgs
    {
        public double x, y;
        public CoordinatesEventArgs(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }





    internal class Ball
    {
        #region fielsd and properties
        double x=0;
        double y=0;
        Rect windowRect;
        bool isMove=false;
        uint width,height;

        double angle = 0;
        #endregion
        public event EventHandler GotScore;
        public event EventHandler MoveEvent;
        public event EventHandler CheckCollisionEvent;
        const uint SPEED = 7;
        static Random random = new Random();
        #region methods
        public void CheckCollision(Rect rect)
        {
            if(x<windowRect.Width/2)
            {
                
            }
            else
            {

            }
            //MessageBox.Show($"Колизия\nleft:{rect.Left}\ntop:{rect.Top}\nright:{rect.Right}\nbottom:{rect.Bottom}");
        }
        protected void Step()
        {
            if(!isMove)
            {
                isMove = true;
                Move();
            }
        }


        public double GetX()
        {
            return x;
        }
        public double GetY()
        {
            return y;
        }

        protected async void Move()
        {
           
            await Task.Run(() =>
            {
                while(isMove)
                {
                    this.x += Math.Cos(angle)* SPEED;
                    this.y += Math.Sin(angle)* SPEED;



                    if(x<0 || x+this.width*2>windowRect.Right)
                    {
                        x-=Math.Cos(angle) * SPEED;
                        if (angle < Math.PI)
                            angle = Math.PI - angle;
                        else
                            angle = Math.PI * 3 - angle;
                    }



                    if(y<0 || y+this.height*3 > windowRect.Bottom)
                    {
                        y-=Math.Sin(angle)*SPEED;
                            angle = Math.PI*2 - angle;
                    }




                    if (angle>Math.PI*2)
                        angle -= ((int)Math.Truncate(angle/Math.PI*2))*Math.PI*2;
                    if(angle < 0)
                        angle=Math.PI*2-angle;

                    //Correct "slow" angle
                    if (angle <= 0.1 && angle >= -0.1)
                        angle = random.Next((int)(Math.PI * 100) + 1) / 100.0;
                    if (angle+Math.PI/2 <= 0.1 && angle + Math.PI / 2 >= -0.1)
                        angle = random.Next((int)(Math.PI * 100) + 1) / 100.0;

                    //Move event
                    if (MoveEvent!=null)
                        MoveEvent(this,new CoordinatesEventArgs(this.x,this.y));
                    //Collision event
                    if (x <= 40 || x + this.width*2+40 >= windowRect.Width)
                        if (CheckCollisionEvent != null)
                            CheckCollisionEvent(this, new CoordinatesEventArgs(this.x, this.y));
                    Thread.Sleep(10);
                }
            });
        }
        #endregion



        public unsafe Ball(double x, double y, double angle, Rect windowRect, uint width, uint height)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.windowRect = windowRect;
            this.width = width;
            this.height = height;
            Step();
        }
    }
}
