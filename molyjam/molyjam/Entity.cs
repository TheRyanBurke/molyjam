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
            set { heading = value; heading.Normalize(); } // Normalize all headings
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

        public void moveEntity(Vector2 vector, List<Entity> entities)
        {
            Vector2 oldPosition = new Vector2(Origin.X, Origin.Y);
            origin.X = MathHelper.Clamp(origin.X + vector.X, 0, Constants.screenWidth);
            origin.Y = MathHelper.Clamp(origin.Y - vector.Y, 0, Constants.screenHeight);

            // Hit detection with other entities (Not players or bullets)
            foreach(Entity e in entities)
            {
             //   if (isCivilianNotPlayer() && !this.Equals(e) && this.detectCollision(e))
             //       origin = oldPosition;
            }

        }

        public Rectangle getDrawArea()
        {
            return getDrawAreaWithOrigin(Origin);
        }

        public Rectangle getDrawAreaWithOrigin(Vector2 o)
        {
            Rectangle area = this.texture.Bounds;
            area.Offset(Convert.ToInt32(o.X), Convert.ToInt32(o.Y));
            return area;
        }

        public Rectangle getBoundingBox()
        {
            double offset = Math.Cos(this.Heading.Y) * Math.Sqrt(2);
            return new Rectangle((int)Origin.X-(int)(0.5*offset), (int)Origin.Y-(int)(0.5*offset), Texture.Width+(int)offset, Texture.Height+(int)offset);
            //return new Rectangle((int)Origin.X, (int)Origin.Y, Texture.Width, Texture.Height);
        }

        public bool detectCollision(Entity e)
        {
            //bool collision = false;
            //Rectangle thisArea = getDrawArea();
            //e.getDrawArea().Intersects(ref thisArea, out collision);
            //e.getBoundingBox().Intersects(ref thisArea, out collision);
            //return collision;
            Rectangle thisArea = getBoundingBox();
            if(e is EnvironmentalObject)
                return ((EnvironmentalObject)e).getDrawArea().Intersects(thisArea);
            return e.getBoundingBox().Intersects(thisArea);

        }

        public bool isCivilianNotPlayer()
        {
            return (this is Civilian && !(this is Player));
        }

        public bool isNotPlayerOrBullet()
        {
            return (!(this is Player) && !(this is Bullet));
        }
    }
}
