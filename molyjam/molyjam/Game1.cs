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

 
        Song bgm;
        Song introbgm;
        SoundEffect gunshot;
        

        List<Civilian> civilians;
        Player player;
        List<Bullet> bullets;
        List<EnvironmentalObject> envObjects;

        int score;
        int combo;
        int combo_counter;

        Texture2D targetBorder;
        Texture2D blah;
        Texture2D civ_tex1;
        Texture2D civ_tex1_aimed;
        Texture2D player_tex;
        Texture2D player_shot_once;
        Texture2D player_shot_twice;
        Texture2D player_pointing;
        Texture2D bullet_tex;
        Texture2D gameover_tex;
        Texture2D gameover_suicide_tex;
        Texture2D env_tex;
        Texture2D background;
        Texture2D truck_red;
        Texture2D intro;

        bool gameover;
        bool gameover_suicide;
        bool isStateKeyPressed;

        bool introState;

        int shootTimer;

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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            civilians = new List<Civilian>();
            bullets = new List<Bullet>();
            envObjects = new List<EnvironmentalObject>();

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
            player_tex = Content.Load<Texture2D>("Suicide");
            player_shot_once = Content.Load<Texture2D>("ShotOnce");
            player_shot_twice = Content.Load<Texture2D>("ShotTwice");
            player_pointing = Content.Load<Texture2D>("pointing");
            civ_tex1 = Content.Load<Texture2D>("civ1-32");
            civ_tex1_aimed = Content.Load<Texture2D>("civ1-32-aimed");
            bullet_tex = Content.Load<Texture2D>("bullet");
            gameover_tex = Content.Load<Texture2D>("gameover");
            gameover_suicide_tex = Content.Load<Texture2D>("gameover-suicide");
            background = Content.Load<Texture2D>("background");
            truck_red = Content.Load<Texture2D>("truck-red");
            intro = Content.Load<Texture2D>("intro");

            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });

            env_tex = new Texture2D(GraphicsDevice, 1, 1);
            env_tex.SetData(new[] { Color.Tan });

            font = Content.Load<SpriteFont>("SpriteFont1");

            bgm = Content.Load<Song>("cautious-path");
            introbgm = Content.Load<Song>("molyjam-bgm");

            gunshot = Content.Load<SoundEffect>("colt45");
            Constants.ricochet = Content.Load<SoundEffect>("bulletricochet-01");
            Constants.scream = Content.Load<SoundEffect>("WilhelmScream");

            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });

            blah = new Texture2D(GraphicsDevice, 1, 1);
            blah.SetData(new[] { Color.White });

            font = Content.Load<SpriteFont>("SpriteFont1");

            introState = true;
            MediaPlayer.Play(introbgm);
        }

        protected void initGameObjects()
        {
            shootTimer = 0;
            player = new Player(new Vector2(1050f, 400f), player_tex);

            civilians.Clear();
            civilians.Add(new Civilian(new Vector2(100f, 300f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(320f, 290f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(400f, 400f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(500f, 400f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(600f, 400f), civ_tex1));

            bullets.Clear();

            envObjects.Clear();
            envObjects.Add(new EnvironmentalObject(new Vector2(850f, 210f), truck_red, new Rectangle(0, 0, 300, 130))); // car
            envObjects.Add(new EnvironmentalObject(new Vector2(80f, 390f), truck_red, new Rectangle(0, 0, 300, 130))); // car
            //envObjects.Add(new EnvironmentalObject(new Vector2(0f, 0f), env_tex, new Rectangle(0, 0, 438, 193)));
            //envObjects.Add(new EnvironmentalObject(new Vector2(0f, 504f), env_tex, new Rectangle(0, 0, 446, 220)));

            gameover = false;
            gameover_suicide = false;
            isStateKeyPressed = false;

            score = 0;
            combo = 0;
            combo_counter = 0;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgm);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            targetBorder.Dispose();
            blah.Dispose();
            civ_tex1.Dispose();
            civ_tex1_aimed.Dispose();
            player_tex.Dispose();
            player_pointing.Dispose();
            bullet_tex.Dispose();
            gameover_tex.Dispose();
            gameover_suicide_tex.Dispose();
            env_tex.Dispose();
            background.Dispose();
            bgm.Dispose();
            gunshot.Dispose();
            Constants.scream.Dispose();
            Constants.ricochet.Dispose();
            intro.Dispose();
            introbgm.Dispose();

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

            if (kbs.GetPressedKeys().Contains(Keys.D1) || gps.IsButtonDown(Buttons.Start))
            {
                initGameObjects();
                introState = false;
            }


            

            if (!gameover && !introState)
            {

                #region KeyboardMovementBlock
                // Keyboard movement block. Added temporarily for debugging.
                // Does not reset leftStick between updates, since it is local and initialized to Vector2.Zero (gps.Thumbsticks.Left with no gamepad)
                Keys[] keyList = Keyboard.GetState().GetPressedKeys();
                if (keyList.Length == 0)
                    leftStick *= player.KeyboardSpeed;
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
                        case Keys.OemPlus:
                            if (!isStateKeyPressed)
                                Constants.SHOOT_INTERVAL += Constants.SHOOT_INTERVAL_ADJUSTMENT;
                            isStateKeyPressed = true;
                            //System.Diagnostics.Debug.WriteLine("State key on, shoot interval: {0}", Constants.SHOOT_INTERVAL);
                            break;
                        case Keys.OemMinus:
                            if (!isStateKeyPressed && Constants.SHOOT_INTERVAL > Constants.SHOOT_INTERVAL_ADJUSTMENT)
                                Constants.SHOOT_INTERVAL -= Constants.SHOOT_INTERVAL_ADJUSTMENT;
                            isStateKeyPressed = true;
                            //System.Diagnostics.Debug.WriteLine("State key on, shoot interval: {0}", Constants.SHOOT_INTERVAL);
                            break;
                    }
                }
                if (!keyList.Contains(Keys.OemPlus) && !keyList.Contains(Keys.OemMinus) && isStateKeyPressed)
                    isStateKeyPressed = false; //System.Diagnostics.Debug.WriteLine("State key off, shoot interval: {0}", Constants.SHOOT_INTERVAL); }

                if (leftStick != Vector2.Zero)
                    player.Heading = leftStick; // Make sure heading reflects current stick direction
                // Keyboard movement block end 
                #endregion

                List<Entity> allEntities = new List<Entity>();
                allEntities.AddRange(civilians);
                allEntities.AddRange(bullets);
                allEntities.AddRange(envObjects);
                allEntities.Add(player);

                //move player
                player.move(leftStick, allEntities);

                //move civilians
                foreach (Civilian civilian in civilians)
                {
                    civilian.Update(player, allEntities);
                }

                //acquireTarget should come before bullet updates
                player.acquireTarget(civilians);

                // bullet.update() returns true if the bullet has run out of ricochets
                List<Bullet> remainingBullets = new List<Bullet>();
 
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
                shootTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (shootTimer > Constants.SHOOT_INTERVAL)
                {
                    shootTimer = 0;

                    Vector2 bulletHeading = player.shoot();
                    if (!(player.Target is Player))
                        bullets.Add(new Bullet(player.Origin, bullet_tex, bulletHeading, Constants.DEFAULT_BULLET_RICOCHETS,leftStick));
                    else
                        player.getShot();

                    gunshot.Play();
                }

                int numDeadCivilians = 0;
                foreach (Civilian c in civilians)
                {
                    if (c.CivilianState == Civilian.CivilianStates.Dead)
                        numDeadCivilians++;
                }

                if (numDeadCivilians >= Constants.MAX_DEAD_CIVILIANS || player.Health <= 0)
                {
                    if (player.Health <= 0)
                        gameover_suicide = true;
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

            if (introState)
            {
                spriteBatch.Draw(intro, intro.Bounds, Color.White);
            }
            else
            {

                spriteBatch.Draw(background, background.Bounds, Color.White);

                foreach (Civilian c in allPeople)
                {
                    double rotationAngle = 0.0f;
                    if (c.Heading.X >= 0)
                        rotationAngle = Math.Acos(c.Heading.Y);
                    else
                        rotationAngle = (Math.PI * 2) - Math.Acos(c.Heading.Y);
                    //spriteBatch.Draw(c.Texture, c.getDrawArea(), Color.White); //No rotation


                    //spriteBatch.Draw(c.Texture, destRect, null, Color.White, (float)rotationAngle, new Vector2(c.Heading.X + 0.5f * c.Texture.Width, c.Heading.Y + 0.5f * c.Texture.Height), SpriteEffects.None, 0);
                    // for debugging - shows origin
                    //spriteBatch.Draw(bullet_tex, new Rectangle((int)c.Origin.X, (int)c.Origin.Y, bullet_tex.Width, bullet_tex.Height), Color.White);
                    //if (player.Target.Equals(c))
                    //{
                    //    if (c.Shot)
                    //        targetBorder.SetData(new[] { Color.Red });
                    //    else
                    //        targetBorder.SetData(new[] { Color.White });
                    //    Rectangle border = c.getDrawArea();
                    //    border.X -= 5;
                    //    border.Y -= 5;
                    //    border.Width += 10;
                    //    border.Height += 10;
                    //    spriteBatch.Draw(targetBorder, border, null, Color.White, (float)rotationAngle, 
                    //        new Vector2(c.Heading.X + 0.5f * border.Width, c.Heading.Y + 0.5f * border.Height), SpriteEffects.None, 0);
                    //}
                    Texture2D civTexToUse = c.Texture;
                    if (player.Target.Equals(c) && !(c is Player))
                        civTexToUse = civ_tex1_aimed;
                    if (c is Player)
                    {
                        if (!((Player)c).Target.Equals(c))
                        {
                            civTexToUse = player_pointing;
                            
                        }
                        else if (((Player)c).Health == 3)
                        {
                            civTexToUse = player_tex;
                            rotationAngle = 0;
                        }
                        else if (((Player)c).Health == 2)
                        {
                            civTexToUse = player_shot_once;
                            rotationAngle = 0;
                        }
                        else if (((Player)c).Health < 2)
                        {
                            civTexToUse = player_shot_twice;
                            rotationAngle = 0;
                        }
                    }
                    Rectangle destRect = c.getDrawArea();
                    destRect.X += c.Texture.Width / 2;
                    destRect.Y += c.Texture.Height / 2;

                    spriteBatch.Draw(civTexToUse, destRect, null, Color.White, (float)rotationAngle, new Vector2(c.Heading.X + 0.5f * civTexToUse.Width, c.Heading.Y + 0.5f * civTexToUse.Height), SpriteEffects.None, 0);
                }

                foreach (EnvironmentalObject e in envObjects)
                {
                    spriteBatch.Draw(e.Texture, e.getDrawArea(), Color.White);
                }

                foreach (Bullet b in bullets)
                {
                    double rotationAngle = 0.0f;
                    if (b.Heading.X >= 0)
                        rotationAngle = Math.Acos(b.Heading.Y);
                    else
                        rotationAngle = (Math.PI * 2) - Math.Acos(b.Heading.Y);
                    spriteBatch.Draw(b.Texture, b.getDrawArea(), null, Color.White, (float)rotationAngle, new Vector2(5 + b.Heading.X, 5 + b.Heading.Y), SpriteEffects.None, 0);
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
                if (combo_counter > 0)
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

                float fraction = (float)shootTimer / (float)Constants.SHOOT_INTERVAL;
                Rectangle timer = new Rectangle(400, 30, (int)(100 - (100 * fraction)), 15);
                spriteBatch.Draw(blah, timer, Color.Red);

                String health = "Health: " + player.Health;
                spriteBatch.DrawString(font, health, new Vector2(5, 25), Color.Black);

                if (gameover)
                {
                    Rectangle area = gameover_tex.Bounds;
                    area.Offset(Constants.screenWidth / 2 - area.Width/2, Constants.screenHeight / 2 - area.Height/2);
                    if (gameover_suicide)
                        spriteBatch.Draw(gameover_suicide_tex, area, Color.White);
                    else
                        spriteBatch.Draw(gameover_tex, area, Color.White);
                }

            }
            spriteBatch.End();
            #endregion drawStuffs
            base.Draw(gameTime);
        }


    }
}
