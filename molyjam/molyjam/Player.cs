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

        int health;
        public int Health
        {
            get { return health; }
            set 
            {
                // Clamps index to range (0..PLAYER_SPEED_HEALTH.Length-1) to keep indices in bounds.
                // int i = value < 0 ? 0 : (value > (Constants.PLAYER_SPEED_HEALTH.Length - 1) ? Constants.PLAYER_SPEED_HEALTH.Length - 1 : value);
                health = value;
                value = (int)MathHelper.Clamp(value-1,0,Constants.PLAYER_SPEED_HEALTH.Length - 1); // more clear
                Speed = Constants.PLAYER_SPEED_HEALTH[value];
                KeyboardSpeed = Constants.PLAYER_SPEED_HEALTH[value];
                //System.Diagnostics.Debug.WriteLine("Current health, speeds (H,S,K): {0}  {1}  {2}",Health, Speed, KeyboardSpeed);
            }
        }

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
            Health = 3;
            Heading = new Vector2(0, 1);
        }

        public void move(Vector2 vector, List<Entity> entities)
        {
            Vector2 oldposition = Origin;
            this.moveEntity(vector, entities);
            foreach (Entity e in entities)
            {
                if (!this.Equals(e) && this.detectCollision(e))
                {
                    Origin = oldposition;
                    break;
                }

            }
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
            double y = (this.Origin.Y - target.Origin.Y);// / Math.Sqrt(target.Origin.Y * target.Origin.Y + this.Origin.Y * this.Origin.Y);
            Vector2 vectorToTarget = new Vector2((float)x, (float)y);
            return vectorToTarget;
        }

        private float distanceToCivilian(Civilian c)
        {
            return (this.Origin - c.Origin).Length();
        }

        public new void getShot()
        {
            Shot = true;
            Health--;
        }

    }
}
