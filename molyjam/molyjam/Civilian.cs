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


        bool shot;
        public bool Shot
        {
            get { return shot; }
            set { shot = value; }
        }

        public CivilianStates CivilianState
        {
            get { return civilianState; }
            set
            {
                if (civilianState != value)
                {
                    #region Civilian_Change_Behavoir
                    if (civilianState == CivilianStates.Inactive && value != CivilianStates.Default) { return; } // Inactive civilians should go to Default state
                    switch (value)
                    {
                        case CivilianStates.Alarmed:
                            speed = 2.0f;
                            break;
                        default:
                            speed = 1.0f;
                            break;
                    }
                    #endregion
                    civilianState = value; 
                }
            }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Civilian(Vector2 origin, Texture2D texture)
            : this(origin, new Vector2(0,1), CivilianStates.Default, texture)
        {

        }

        public Civilian(Vector2 origin, Vector2 heading, CivilianStates state, Texture2D texture, float speed = 1.0f) // Constructor Override, allows the state to be set
            : base(origin, texture)
        {
            lifeTime = Stopwatch.StartNew();

            this.Heading = heading;
            this.civilianState = state;
            this.speed = speed;
            shot = false;

            Random gen = new Random();
            headingChangeMillis = gen.Next(500,1000);
        }

        public void getShot()
        {
            shot = true;
        }

        public void moveCivilian()
        {
            this.moveEntity(Heading * speed);
            float modHeadingX = 1;
            float modHeadingY = 1;
            if (Origin.X <= 0 || Origin.X >= (Constants.screenWidth - Texture.Width))
                modHeadingX = -1;
            if (Origin.Y <= 0 || Origin.Y >= (Constants.screenHeight - Texture.Height))
                modHeadingY = -1;
            Vector2 newHeading = new Vector2(modHeadingX * Heading.X, modHeadingY * Heading.Y);
            Heading = newHeading;

        }

        public void Update(Player player)
        {

            #region Civilian_State_Check
            // Set state to alarmed if player is close
            if (Math.Abs((Origin - player.Origin).Length()) < 200)
            {
                CivilianState = Civilian.CivilianStates.Alarmed;
            }
            #endregion

            if (lifeTime.ElapsedMilliseconds >= headingChangeMillis)
            {
                Random gen = new Random();
                double rnd;
                Vector2 rot;
                #region Civilian_Behavior_Rules
                switch (this.civilianState)
                {
                    case CivilianStates.Default:
                        rnd = gen.Next(0,46)-22.5;
                        rnd *= Math.PI / 180;                   // Needs to convert degrees to radians. Perhaps a static helper method in constants?
                        rot = this.Heading;
                        break;
                    case CivilianStates.Alarmed:
                        rnd = gen.Next(0, 91) - 45;
                        rnd *= Math.PI / 180;                   // Needs to convert degrees to radians. Perhaps a static helper method in constants?
                        rot = this.Origin - player.Origin;
                        break;
                    default:
                        rnd = 0.0;
                        rot = this.Heading;
                        break;
                }
                #endregion

                rot.X = (rot.X * (float)Math.Cos(rnd)) + (rot.Y * (float)Math.Sin(rnd));
                rot.Y = (rot.Y * (float)Math.Cos(rnd)) - (rot.X * (float)Math.Sin(rnd));
                this.Heading = rot;
                headingChangeMillis = lifeTime.ElapsedMilliseconds + gen.Next(500, 1000);
            }

            moveCivilian();
        }
    }
}
