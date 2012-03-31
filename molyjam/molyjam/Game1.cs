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

        List<Civilian> civilians;
        Player player;

        Texture2D targetBorder;

       

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
            Texture2D tex = Content.Load<Texture2D>("player");
            Texture2D civ_tex1 = Content.Load<Texture2D>("civ1");

            player = new Player(new Vector2(50f, 50f), tex);

            civilians.Add(new Civilian(new Vector2(100f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(200f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(300f, 100f), civ_tex1));
            civilians.Add(new Civilian(new Vector2(500f, 50f), new Vector2(0,-1), Civilian.CivilianStates.Default,civ_tex1));

            targetBorder = new Texture2D(GraphicsDevice, 1, 1);
            targetBorder.SetData(new[] { Color.White });
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
            
            player.moveEntity(leftStick);
            foreach (Civilian civilian in civilians)
            {
                civilian.Update();
            }

            player.acquireTarget(civilians);

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
                    Rectangle border = c.getDrawArea();
                    border.X -= 5;
                    border.Y -= 5;
                    border.Width += 10;
                    border.Height += 10;
                    spriteBatch.Draw(targetBorder, border, Color.White);
                }
                spriteBatch.Draw(c.Texture, c.getDrawArea(), Color.White);
            }

            spriteBatch.End();
            #endregion drawStuffs
            base.Draw(gameTime);
        }
    }
}
