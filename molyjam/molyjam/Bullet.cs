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
        bool expired = false;
        public bool Expired
        {
            get { return expired; }
            set { expired = value; }
        }

        Stopwatch lifetime;
        int ricochetsRemaining;


        public Bullet(Vector2 origin, Texture2D texture, Vector2 heading, int ricochets, Vector2 platformInertia)
            : base(origin, texture)
        {
            Heading = heading + Constants.BULLET_INERTIAL_INFLUENCE*platformInertia;
            ricochetsRemaining = ricochets;
            lifetime = Stopwatch.StartNew();
        }

        public bool update(List<Entity> entities)
        {
            return moveBullet(entities);
        }

        public bool moveBullet(List<Entity> entities)
        {
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
                Constants.ricochet.Play();
            }
            if (Origin.Y <= 0 || Origin.Y >= (Constants.screenHeight - Texture.Height))
            {
                if (ricochetsRemaining > 0)
                {
                    modHeadingY = -1;
                }
                ricochetsRemaining--;
                Constants.ricochet.Play();
            }

            foreach (Entity e in entities)
            {
                if (!(e.Equals(this)) && this.detectCollision(e))
                {
                    if (e is Player && lifetime.Elapsed.Milliseconds > 500)
                    {
                        ((Player)e).getShot();
                        Constants.scream.Play();
                        return true;
                    }
                    if (e is Civilian && !(e is Player))
                    {
                        ((Civilian)e).getShot();
                        Constants.scream.Play();
                        return true;
                    }
                    //TODO may need to revisit so bullet can ricochet off non-civilian entities
                    if (e is EnvironmentalObject)
                    {
                        if (ricochetsRemaining > 0)
                        {
                            //TODO probably need a better way to determine how the bullet should ricochet, though this works for now
                            if (Origin.X > e.Origin.X && Origin.X < e.Origin.X + ((EnvironmentalObject)e).getDrawArea().Width-4)
                                modHeadingY = -1;
                            else
                                modHeadingX = -1;
                        }
                        ricochetsRemaining--;
                        Constants.ricochet.Play();
                    }
                }
            }
            Vector2 newHeading = new Vector2(modHeadingX * Heading.X, modHeadingY * Heading.Y);
            Heading = newHeading;
            if (ricochetsRemaining < 0)
                expired = true;

            return expired;
        }
    }
}
