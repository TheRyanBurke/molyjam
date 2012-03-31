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
        public enum CivilianStates { Inactive, Default, Alarmed, Dead } // enums to show possible states of civilian entity, move to constants.cs

        CivilianStates civilianState;   // Civilian-specific members
        float speed;

        public Civilian(Vector2 origin, Texture2D texture)
            : base(origin, texture)
        {

        }

        public Civilian(Vector2 origin, Vector2 heading, CivilianStates state, Texture2D texture, float speed = 1.0f) // Constructor Override, allows the state to be set
            : base(origin, texture)
        {
            this.Heading = heading;
            this.civilianState = state;
            this.speed = speed;
        }

        public void Update()
        {
            this.moveEntity(this.Heading * speed);
        }
    }
}
