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

        int timeOfLastShot = 0;

        Texture2D civ_tex1;
        Texture2D player_tex;
        Texture2D bullet_tex;
       

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

            player = new Player(new Vector2(50f, 50f), player_tex);

            civilians.Add(new Civilian(new Vector2(100f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(200f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(300f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(400f, 50f), civ_tex1));

            bullets = new List<Bullet>();

            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });

            font = Content.Load<SpriteFont>("SpriteFont1");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Contains(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            KeyboardState kbs = Keyboard.GetState();
            Vector2 leftStick = gps.ThumbSticks.Left;

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
            }
            bullets = remainingBullets;            

            //bullet fire should be last event in engine loop
            
            if (gameTime.TotalGameTime.Milliseconds % Constants.SHOOT_INTERVAL == 0)
            {
                Vector2 bulletHeading = player.shoot();
                if (!(player.Target is Player))
                    bullets.Add(new Bullet(player.Origin, bullet_tex, bulletHeading, Constants.DEFAULT_BULLET_RICOCHETS));
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

            spriteBatch.End();
            #endregion drawStuffs
            base.Draw(gameTime);
        }
    }
}
