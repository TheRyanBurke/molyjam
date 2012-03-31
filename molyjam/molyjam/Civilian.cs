using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace molyjam
{
    class Civilian : Entity
    {

        bool shot;
        public bool Shot
        {
            get { return shot; }
            set { shot = value; }
        }

        public Civilian(Vector2 origin, Texture2D texture)
            : base(origin, texture)
        {
            shot = false;
        }

        public void getShot()
        {
            shot = true;
        }
    }
}
