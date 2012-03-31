using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace molyjam
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        List<Civilian> civilians;
        Player player;
        List<Bullet> bullets;

        Texture2D targetBorder;
        Texture2D blah;

        int timeOfLastShot = 0;
        int score = 0;
        int combo = 0;
        int combo_counter = 0;


        

        Texture2D civ_tex1;
        Texture2D player_tex;
        Texture2D bullet_tex;
        Texture2D gameover_tex;

        bool gameover;
       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            civilians = new List<Civilian>();
            bullets = new List<Bullet>();

            Constants.screenWidth = GraphicsDevice.Viewport.Width;
            Constants.screenHeight = GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player_tex = Content.Load<Texture2D>("player");
            civ_tex1 = Content.Load<Texture2D>("civ1-32");
            bullet_tex = Content.Load<Texture2D>("bullet");
            gameover_tex = Content.Load<Texture2D>("gameover");

            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });

            font = Content.Load<SpriteFont>("SpriteFont1");

            initGameObjects();
        }

        protected void initGameObjects()
        {
            player = new Player(new Vector2(50f, 50f), player_tex);

            civilians.Clear();
            civilians.Add(new Civilian(new Vector2(100f, 300f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(200f, 300f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(300f, 300f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(400f, 350f), civ_tex1));

            bullets.Clear();
            bullets = new List<Bullet>();


            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });

            blah = new Texture2D(GraphicsDevice, 1, 1);
            blah.SetData(new[] { Color.White });

            font = Content.Load<SpriteFont>("SpriteFont1");

            gameover = false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            targetBorder.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            KeyboardState kbs = Keyboard.GetState();
            Vector2 leftStick = gps.ThumbSticks.Left;

            // Allows the game to exit
            if (gps.Buttons.Back == ButtonState.Pressed || kbs.GetPressedKeys().Contains(Keys.Escape))
                this.Exit();

            if (kbs.GetPressedKeys().Contains(Keys.D1))
                initGameObjects();


            #region KeyboardMovementBlock
            // Keyboard movement block. Added temporarily for debugging.
            // Does not reset leftStick between updates, since it is local and initialized to Vector2.Zero (gps.Thumbsticks.Left with no gamepad)
            Keys[] keyList = Keyboard.GetState().GetPressedKeys();
            for (int i = 0; i < keyList.Length; i++)
            {
                switch (keyList[i])
                {
                    case Keys.Down:
                        leftStick += new Vector2(0f, -1 * player.KeyboardSpeed);
                        break;
                    case Keys.Up:
                        leftStick += new Vector2(0f, player.KeyboardSpeed);
                        break;
                    case Keys.Left:
                        leftStick += new Vector2(-1 * player.KeyboardSpeed, 0f);
                        break;
                    case Keys.Right:
                        leftStick += new Vector2(player.KeyboardSpeed, 0f);
                        break;
                }
            }
            // Keyboard movement block end 
            #endregion

            if (!gameover)
            {
                //move player
                player.moveEntity(leftStick);

                //move civilians
                foreach (Civilian civilian in civilians)
                {
                    civilian.Update(player);
                }

                //acquireTarget should come before bullet updates
                player.acquireTarget(civilians);

                // bullet.update() returns true if the bullet has run out of ricochets
                List<Bullet> remainingBullets = new List<Bullet>();
                List<Entity> allEntities = new List<Entity>();
                allEntities.AddRange(civilians);
                allEntities.AddRange(bullets);
                allEntities.Add(player);
                foreach (Bullet b in bullets)
                {
                    if (!b.update(allEntities))
                        remainingBullets.Add(b);
                    else if (b.Expired)
                    {
                        combo_counter += 1;
                        if (combo_counter == 3)
                        {
                            combo++;
                            combo_counter = 0;
                        }
                        score += 100 * (1 + combo);
                    }
                    else
                    {
                        combo = 0;
                        combo_counter = 0;
                    }
                }
                bullets = remainingBullets;

                //bullet fire should be last event in engine loop

                if (gameTime.TotalGameTime.Milliseconds % Constants.SHOOT_INTERVAL == 0)
                {
                    Vector2 bulletHeading = player.shoot();
                    if (!(player.Target is Player))
                        bullets.Add(new Bullet(player.Origin, bullet_tex, bulletHeading, Constants.DEFAULT_BULLET_RICOCHETS));
                    else
                        player.getShot();
                }

                int numDeadCivilians = 0;
                foreach (Civilian c in civilians)
                {
                    if (c.CivilianState == Civilian.CivilianStates.Dead)
                        numDeadCivilians++;
                }

                if (numDeadCivilians >= Constants.MAX_DEAD_CIVILIANS || player.Health <= 0)
                {
                    gameover = true;
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            List<Civilian> allPeople = new List<Civilian>();
            allPeople.Add(player);
            allPeople.AddRange(civilians);

            #region drawStuffs
            spriteBatch.Begin();

            foreach(Civilian c in allPeople) {
                if (player.Target.Equals(c))
                {
                    if (c.Shot)
                        targetBorder.SetData(new[] { Color.Red });
                    else
                        targetBorder.SetData(new[] { Color.White });
                    Rectangle border = c.getDrawArea();
                    border.X -= 5;
                    border.Y -= 5;
                    border.Width += 10;
                    border.Height += 10;
                    spriteBatch.Draw(targetBorder, border, Color.White);
                }

                spriteBatch.Draw(c.Texture, c.getDrawArea(), Color.White);
            }

            foreach (Bullet b in bullets)
            {
                //spriteBatch.Draw(b.Texture, b.getDrawArea(), null, Color.White, (float)(Math.Atan2(b.Origin.Y, b.Origin.X) / (2 * Math.PI)), b.Origin, SpriteEffects.None, 0);
                spriteBatch.Draw(b.Texture, b.getDrawArea(), Color.White);
            }


            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(font, "Combo:" + combo.ToString(), new Vector2(200, 5), Color.Black);
            Rectangle combo1_border = new Rectangle(205, 30, 10, 10);
            Rectangle combo2_border = new Rectangle(225, 30, 10, 10);
            Rectangle combo3_border = new Rectangle(245, 30, 10, 10);
            Rectangle combo1 = new Rectangle(207, 32, 6, 6);
            Rectangle combo2 = new Rectangle(227, 32, 6, 6);
            Rectangle combo3 = new Rectangle(247, 32, 6, 6);
            spriteBatch.Draw(blah, combo1_border, Color.Black);
            spriteBatch.Draw(blah, combo2_border, Color.Black);
            spriteBatch.Draw(blah, combo3_border, Color.Black);
            if(combo_counter > 0)
                spriteBatch.Draw(blah, combo1, Color.Red);
            else
                spriteBatch.Draw(blah, combo1, Color.White);
            if (combo_counter > 1)
                spriteBatch.Draw(blah, combo2, Color.Red);
            else
                spriteBatch.Draw(blah, combo2, Color.White);
            if (combo_counter > 2)
                spriteBatch.Draw(blah, combo3, Color.Red);
            else
                spriteBatch.Draw(blah, combo3, Color.White);


            String health = "Health: " + player.Health;
            spriteBatch.DrawString(font, health, new Vector2(10, 10), Color.White);

            if (gameover)
            {
                Rectangle area = gameover_tex.Bounds;
                area.Offset(Constants.screenWidth / 2, Constants.screenHeight / 2);
                spriteBatch.Draw(gameover_tex, area, Color.White);
            }

            spriteBatch.End();
            #endregion drawStuffs
            base.Draw(gameTime);
        }


    }
}
