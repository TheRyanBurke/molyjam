using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace molyjam
{
    class Player : Civilian
    {

        float keyboardSpeed = 5.0f;
        public float KeyboardSpeed
        {
            get { return keyboardSpeed; }
            set { keyboardSpeed = value; }
        }

        Civilian target;
        public Civilian Target
        {
            get { return target; }
            set { target = value; }
        }

        public Player(Vector2 origin, Texture2D texture)
            : base(origin, texture)
        {
            target = this;
        }

        public void acquireTarget(List<Civilian> civs)
        {
            target = civs.ElementAt(0);
            foreach (Civilian c in civs)
            {
                if ( distanceToCivilian(c) < distanceToCivilian(target) )
                {
                    target = c;
                }
            }
            
            if (distanceToCivilian(target) > Constants.TARGET_RANGE)
            {
                target = this;
            }
        }

        /**
         * Return Vector2 from player to target
         */
        public Vector2 shoot()
        {
            if(target is Player)
                target.getShot();  
            
            double x = (target.Origin.X - this.Origin.X);// / Math.Sqrt(target.Origin.X * target.Origin.X + this.Origin.X * this.Origin.X);
            double y = (target.Origin.Y - this.Origin.Y);// / Math.Sqrt(target.Origin.Y * target.Origin.Y + this.Origin.Y * this.Origin.Y);
            Vector2 vectorToTarget = new Vector2((float)x, (float)y);
            return vectorToTarget;
        }

        private float distanceToCivilian(Civilian c)
        {
            return (this.Origin - c.Origin).Length();
        }

    }
}
