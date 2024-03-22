#region LICENSE
/*
Copyright(C) 2024 Andrew Lerdal

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
#endregion

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
        public static bool loaded = false;
        public static bool intialized = false;
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
            ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;
            gd = graphics.GraphicsDevice;

            uiManager = new UiManager();
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
                uiManager.Init();
            }
            //update logic
            if (WasResized)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width; //1920;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height; //1080;

                graphics.ApplyChanges();

                WasResized = false;
            }

            base.Update(gameTime);
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

            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            //ImGui Begin
            GuiRenderer.BeginLayout(gameTime);

            //Render UI
            uiManager.Draw();

            //ImGui End
            GuiRenderer.EndLayout();
            spriteBatch.End();
        }
    }
}
