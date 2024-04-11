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

        public double angle = 0;
        #endregion
        public event EventHandler GotScore;
        public event EventHandler MoveEvent;
        public event EventHandler CheckCollisionEvent;
        const uint SPEED = 7;
        static Random random = new Random();
        #region methods
        private void CheckCornerCollision()
        {
            if(x<0 || x>windowRect.Width)
            {
                if(GotScore!=null)
                    GotScore(this, new CoordinatesEventArgs(x,y));
                x = windowRect.Width / 2;
                y = windowRect.Height / 2;
                angle=random.Next((int)4) * Math.PI / 2 + Math.PI / 6;
            }
            





            /*if (x < 0 || x + this.width * 2 > windowRect.Right)
            {
                //x -= Math.Cos(angle) * SPEED;
                if (angle < Math.PI)
                    angle = Math.PI - angle;
                else
                    angle = Math.PI * 3 - angle;
            }*/

            

            if (y < 30 || y + this.height * 3 > windowRect.Bottom)
            {
                angle = Math.PI * 2 - angle;
                y += Math.Sin(angle) * SPEED;
            }
        }
        public void CheckCollision(Rect rect)
        {
            //higher
            if ((y-Math.Sin(angle)*SPEED<rect.Top && y+height>=rect.Top)
                &&
                (x+width>rect.Left && x<rect.Left+rect.Width))
            {
                angle = Math.PI * 2 - angle;
                this.x += Math.Cos(angle) * SPEED;
                this.y += Math.Sin(angle) * SPEED;
            }
            else
            //lower
            if ((y - Math.Sin(angle) * SPEED > rect.Top+rect.Height && y <= rect.Top+rect.Height)
                &&
                (x + width > rect.Left && x < rect.Left + rect.Width))
            {
                angle = Math.PI * 2 - angle;
                this.x += Math.Cos(angle) * SPEED;
                this.y += Math.Sin(angle) * SPEED;
            }
            else

            //left
            if((x-Math.Cos(angle)*SPEED<rect.Left && x+width>=rect.Left)
                &&
                (y+height>=rect.Top && y<=rect.Top+rect.Height))
            {
                if (angle < Math.PI)
                    angle = Math.PI - angle;
                else
                    angle = Math.PI * 3 - angle;

                this.x += Math.Cos(angle) * SPEED;
                this.y += Math.Sin(angle) * SPEED;
            }
            else
            //right
            if ((y + height >= rect.Top && y <= rect.Top + rect.Height))
            if((x - Math.Cos(angle) * SPEED >= rect.Left+rect.Width && x <= rect.Left+rect.Width))
            {
                if (angle < Math.PI)
                    angle = Math.PI - angle;
                else
                    angle = Math.PI * 3 - angle;

                this.x += Math.Cos(angle) * SPEED;
                this.y += Math.Sin(angle) * SPEED;
            }

        }
        protected void Step()
        {
            if(!isMove)
            {
                isMove = true;
                Move();
            }
        }
        protected void CheckAngle()
        {
            while(angle<0)
                angle += Math.PI*2;
            while(angle>=Math.PI*2)
                angle -= ((int)Math.Truncate(angle / Math.PI * 2)) * Math.PI * 2;
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
                while(isMove && MainWindow.notClosed)
                {
                    this.x += Math.Cos(angle)* SPEED;
                    this.y += Math.Sin(angle)* SPEED;
                    CheckCornerCollision();


                    //Correct "slow" angle
                    //CheckAngle();
                    

                    //Move event
                    if (MoveEvent!=null)
                        MoveEvent(this,new CoordinatesEventArgs(this.x,this.y));
                    //Collision event
                    if (CheckCollisionEvent != null)
                        CheckCollisionEvent(this, new CoordinatesEventArgs(this.x, this.y));
                    Thread.Sleep(20);
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
