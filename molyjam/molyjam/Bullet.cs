using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace molyjam
{
    class Bullet : Entity
    {
        float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        int ricochetsRemaining;


        public Bullet(Vector2 origin, Texture2D texture, Vector2 heading)
            : base(origin, texture)
        {
            Heading = heading;
            speed = 10f;
            ricochetsRemaining = 0;
        }

        public bool update()
        {
            return moveBullet();
        }

        public bool moveBullet()
        {
            bool expired = false;
            this.moveEntity(Heading * speed);
            float modHeadingX = 1;
            float modHeadingY = 1;
            if (Origin.X <= 0 || Origin.X >= (Constants.screenWidth - Texture.Width))
            {
                if (ricochetsRemaining > 0)
                {
                    modHeadingX = -1;
                }
                ricochetsRemaining--;
            }
            if (Origin.Y <= 0 || Origin.Y >= (Constants.screenHeight - Texture.Height))
            {
                if (ricochetsRemaining > 0)
                {
                    modHeadingY = -1;
                }
                ricochetsRemaining--;
            }
            Vector2 newHeading = new Vector2(modHeadingX * Heading.X, modHeadingY * Heading.Y);
            Heading = newHeading;
            if (ricochetsRemaining < 0)
                expired = true;
            return expired;
        }
    }
}
