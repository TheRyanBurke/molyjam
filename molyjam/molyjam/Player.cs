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

        float keyboardSpeed = 1.0f;
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
            
            if (distanceToCivilian(target) > 100f)
            {
                target = this;
            }
        }

        public void shoot()
        {
            target.getShot();
        }

        private float distanceToCivilian(Civilian c)
        {
            return (this.Origin - c.Origin).Length();
        }

    }
}
