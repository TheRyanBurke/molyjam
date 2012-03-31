using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace molyjam
{
    class EnvironmentalObject : Entity
    {

        Rectangle area;
        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        public EnvironmentalObject(Vector2 origin, Texture2D texture, Rectangle _area)
            : base(origin, texture)
        {
            area = _area;
        }

        public new Rectangle getDrawArea()
        {
            Rectangle areaWithOffset = area;
            areaWithOffset.Offset(Convert.ToInt32(this.Origin.X), Convert.ToInt32(this.Origin.Y));
            return areaWithOffset;
        }
    }
}
