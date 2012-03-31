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


        public Bullet(Vector2 origin, Texture2D texture, Vector2 heading)
            : base(origin, texture)
        {
            Heading = heading;
            speed = 10f;
        }

        public void update()
        {
            moveEntity(Heading * speed); 
        }
    }
}
