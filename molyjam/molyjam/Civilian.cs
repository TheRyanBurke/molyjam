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
        enum CivilianStates { Inactive, Default, Alarmed, Dead } // enums to show possible states of civilian entity

        CivilianStates civilianState;   // Civilian-specific members
        float speed;

        public Civilian(Vector2 origin, Texture2D texture)
            : base(origin, texture)
        {

        }

        public Civilian(Vector2 origin, Vector2 heading, CivilianStates state, float speed=1.0f, Texture2D texture) // Constructor Override, allows the state to be set
            : base(origin, texture)
        {
            this.Heading = heading;
            this.civilianState = state;
            this.speed = speed;
        }

        public void Update()
        {

        }
    }
}
