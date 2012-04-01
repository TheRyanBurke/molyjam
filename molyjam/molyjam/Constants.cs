using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace molyjam
{
    public static class Constants
    {
        public static int screenWidth;
        public static int screenHeight;

        public static float BULLET_SPEED = 8f;
        public static int SHOOT_INTERVAL = 20000; //ms

        public static int DEFAULT_BULLET_RICOCHETS = 1;

        public static float TARGET_RANGE = 200f;

        public static int MAX_DEAD_CIVILIANS = 1;

        public static float CIVILIAN_SPEED_ALARMED = 2f;
        public static float CIVILIAN_SPEED_NORMAL = 1f;

        public static float[] PLAYER_SPEED_HEALTH = {1.0f,2.5f,4.0f};
        public static Random gen = new Random();
    }
}
