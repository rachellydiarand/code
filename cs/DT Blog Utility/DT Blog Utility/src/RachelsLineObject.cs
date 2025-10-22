using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_Blog_Utility.src
{
    
    public class RachelsLineObject
    {
        public const decimal DG = (decimal)((decimal)Math.PI / 180);
        public const decimal RD = (decimal)(180 / (decimal)Math.PI);

        private RachelsDrawingFun mParentForm { get; set; }

        private decimal mRotation { get; set; } // degrees
        public decimal RotationSpeed { get; set; } // degrees
        private decimal mX { get; set; }
        private decimal mY { get; set; }
        public decimal RegistrationPercentage { get; set; }
        private decimal mLength { get; set; }
        public decimal mDirection { get; set; }
        public decimal Speed { get; set; }
        public int StageWidth { get; set; }
        public int StageHeight { get; set; }

        public RachelsLineObject(RachelsDrawingFun pParentForm, int pStageWidth, int pStageHeight)
        {
            mParentForm = pParentForm;
            StageWidth = pStageWidth;
            StageHeight = pStageHeight;
        }

        public decimal Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                mRotation = value;
            }
        }

        public decimal X
        {
            get
            {
                return mX;
            }
            set
            {
                mX = value;
            }
        }

        public decimal Y
        {
            get
            {
                return mY;
            }
            set
            {
                mY = value;
            }
        }

        public decimal Length
        {
            get
            {
                return mLength;
            }
            set
            {
                mLength = value;
            }
        }

        public decimal Direction
        {
            get
            {
               return mDirection;
            }
            set
            {
                var v = value;
                // keeping Direction getter to 0-360
                if (v < 0)
                {
                    do
                    {
                        v += 360;
                    } while (v < 0);
                }
                if (v > 360)
                {
                    do
                    {
                        v -= 360;
                    } while (v > 360);
                }
                mDirection = v;
            }
        }

        public void RenderFrame(Graphics g)
        {
            // we'll call this from the parent/main form/stage frame event so that if we pause there, we can pause this and have less timers
            Rotation += RotationSpeed;
            X += (decimal)((decimal)Math.Cos((double)(Direction * DG)) * Speed);
            Y += (decimal)((decimal)Math.Sin((double)(Direction * DG)) * Speed);

            // theoreticall the top right point (thinking a line from top right to bottom left, but could be at any angle)
            // mRegistrationPercentage is the percentage from the theoretical top left of the length to the X/Y point
            Point p1 = new System.Drawing.Point();
            p1.X = (int)(X + (decimal)(((decimal)Math.Cos((double)(Rotation * DG))) * (RegistrationPercentage * Length)));
            p1.Y = (int)(Y + (decimal)(((decimal)Math.Sin((double)(Rotation * DG))) * (RegistrationPercentage * Length)));

            Point p2 = new System.Drawing.Point();
            p2.X = (int)(X + (decimal)(((decimal)Math.Cos((double)(Rotation * DG))) * ((1 - RegistrationPercentage) * Length)));
            p2.Y = (int)(Y + (decimal)(((decimal)Math.Sin((double)(Rotation * DG))) * ((1 - RegistrationPercentage) * Length)));

            if(p1.X > StageWidth || p2.X > StageWidth)
            {
                Direction -= 180;
            }

            if(p1.X < 0 || p2.X < 0)
            {
                Direction += 180;
            }

            if (p1.Y > StageHeight || p2.Y > StageHeight)
            {
                Direction -= 180;
            }

            if (p1.Y < 0 || p2.Y < 0)
            {
                Direction += 180;
            }

            g.DrawLine(new System.Drawing.Pen(Color.Red, 1),
                p1, p2);
        }
    }

   
}
