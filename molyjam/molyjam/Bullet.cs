using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace molyjam
{
    class Bullet : Entity
    {
        Stopwatch lifetime;
        int ricochetsRemaining;


        public Bullet(Vector2 origin, Texture2D texture, Vector2 heading, int ricochets)
            : base(origin, texture)
        {
            Heading = heading;
            ricochetsRemaining = ricochets;
            lifetime = Stopwatch.StartNew();
        }

        public bool update(List<Entity> entities)
        {
            return moveBullet(entities);
        }

        public bool moveBullet(List<Entity> entities)
        {
            bool expired = false;
            this.moveEntity(Heading * Constants.BULLET_SPEED, entities);
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

            foreach (Entity e in entities)
            {
                if (!(e.Equals(this)) && e.detectCollision(this))
                {
                    if (e is Player && lifetime.Elapsed.Milliseconds > 500)
                    {
                        ((Player)e).getShot();
                        return true;
                    }
                    if (e is Civilian && !(e is Player))
                    {
                        ((Civilian)e).getShot();
                        return true;
                    }
                    //TODO may need to revisit so bullet can ricochet off non-civilian entities
                }
            }

            return expired;
        }
    }
}
