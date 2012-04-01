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
            return new Rectangle((int)Origin.X, (int)Origin.Y, Texture.Width, Texture.Height);
        }

        public bool detectCollision(Entity e)
        {
            //bool collision = false;
            //Rectangle thisArea = getDrawArea();
            //e.getDrawArea().Intersects(ref thisArea, out collision);
            //e.getBoundingBox().Intersects(ref thisArea, out collision);
            //return collision;
            Rectangle thisArea = getBoundingBox();
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
