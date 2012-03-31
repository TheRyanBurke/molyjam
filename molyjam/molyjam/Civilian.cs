using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Stopwatch lifeTime;
        long headingChangeMillis;       // Stopwatch.elapsedMilliseconds at which next heading change occurs.


        public Civilian(Vector2 origin, Texture2D texture)
            : base(origin, texture)
        {
            lifeTime = new Stopwatch();
            lifeTime.Reset();

            this.Heading = new Vector2(0, 1);

        }

        public Civilian(Vector2 origin, Vector2 heading, CivilianStates state, Texture2D texture, float speed = 1.0f) // Constructor Override, allows the state to be set
            : base(origin, texture)
        {
            lifeTime = Stopwatch.StartNew();

            this.Heading = heading;
            this.civilianState = state;
            this.speed = speed;

            Random gen = new Random();
            headingChangeMillis = gen.Next(500,1000);
        }

        public void Update()
        {
            this.moveEntity(this.Heading * speed);
            if ((lifeTime.ElapsedMilliseconds >= headingChangeMillis) && this.civilianState == CivilianStates.Default)
            {
                Random gen = new Random();
                double rnd = gen.Next(0,46)-22.5;
                rnd *= Math.PI / 180;                   // Needs to convert degrees to radians. Perhaps a static helper method in constants?
                Vector2 rot = this.Heading;
                rot.X = (rot.X * (float)Math.Cos(rnd)) + (rot.Y * (float)Math.Sin(rnd));
                rot.Y = (rot.Y * (float)Math.Cos(rnd)) - (rot.X * (float)Math.Sin(rnd));
                this.Heading = rot;
                headingChangeMillis = lifeTime.ElapsedMilliseconds + gen.Next(500, 1000);          
            }
        }
    }
}
