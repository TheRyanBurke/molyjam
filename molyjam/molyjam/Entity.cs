using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace molyjam
{
    class Entity
    {
        Vector2 origin;
        Vector2 heading;

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Heading
        {
            get { return heading; }
            set { Vector2 normVector = value; value.Normalize(); heading = value; } // Normalize all headings
        }

        Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Entity(Vector2 _origin, Texture2D _texture)
        {
            origin = _origin;
            texture = _texture;
        }

        public void moveEntity(Vector2 vector)
        {
            origin.X += vector.X;
            origin.Y -= vector.Y;
        }

        public Rectangle getDrawArea()
        {
            Rectangle area = this.texture.Bounds;
            area.Offset(Convert.ToInt32(this.origin.X), Convert.ToInt32(this.origin.Y));
            return area;
        }
    }
}
