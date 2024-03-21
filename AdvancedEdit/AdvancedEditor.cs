using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using MonoGame.ImGuiNet;
using ImGuiNET;
using AdvancedEdit.UI;

using Vec2 = System.Numerics.Vector2;
using Vec3 = System.Numerics.Vector3;
using Vec4 = System.Numerics.Vector4;
using AdvancedEdit.TrackData;

namespace AdvancedEdit
{
    public class AdvancedEditor : Game
    {
        //Editor Vars
        public static bool loaded;
        bool intialized = false;
        UiManager uiManager;
        public static byte[] file = new byte[0];
        public static List<Track> tracks = new List<Track>();

        //MonoGame Vars
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice gd;
        public static SpriteBatch spriteBatch;
        public static ImGuiRenderer GuiRenderer;
        bool WasResized = false;
        

        public AdvancedEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            gd = graphics.GraphicsDevice;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            Window.AllowUserResizing = true; // true;
            Window.ClientSizeChanged += delegate { WasResized = true; };
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
            this.Window.Title = "MonoGame & ImGui.NET";

            GuiRenderer = new ImGuiRenderer(this);
            GuiRenderer.RebuildFontAtlas();

            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            gd = graphics.GraphicsDevice;


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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (loaded && !intialized)
            {
                intialized = true;
                uiManager = new UiManager();
            }
            //update logic
            if (WasResized)
            {
                string new_resolution = resolution[select_res];

                int res_width = int.Parse(new_resolution.Split('x')[0]);
                int res_height = int.Parse(new_resolution.Split('x')[1]);

                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width; //1920;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height; //1080;

                graphics.ApplyChanges();

                WasResized = false;
                current_res = select_res;
            }

            base.Update(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = false;
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //Framerate
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Draw Monogame

            base.Draw(gameTime);


            //ImGui Begin
            GuiRenderer.BeginLayout(gameTime);

            //Render UI
            MenuBar.Draw();
            if (loaded && intialized)
                uiManager.Draw();

            //ImGui End
            GuiRenderer.EndLayout();
        }

        #region DrawMonoGameWindowVariables
        bool show_main_window = true;
        bool exit_app = false;
        int current_res = 0;
        int select_res = 0;
        int render_model = 1;
        string[] resolution = { "1024x768", "1280x720", "1280x960", "1366x768", "1440x1080", "1680x1050", "1600x1200", "1920x1080" };
        Vec4 monogame_color = new Vec4(231.0f / 255.0f, 60.0f / 255.0f, 0.0f / 255.0f, 200.0f / 255.0f);
        Vec4 monogame_framebg = new Vec4(227.0f / 255.0f, 227.0f / 255.0f, 227.0f / 255.0f, 255.0f / 255.0f);
        Vec4 color_black = new Vec4(0.0f / 255.0f, 0.0f / 0.0f, 0.0f / 255.0f, 200.0f / 255.0f);
        Vec4 color_white = new Vec4(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 242.0f / 255.0f);
        #endregion
    }
}
