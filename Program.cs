using MkscEdit.Extract;
using MkscEdit.Types;
using SDL2;
using static SDL2.SDL;
using System;
using System.Runtime.CompilerServices;
namespace MkscEdit;
class Program{
    static unsafe void Main(string[] args){
        byte[] file = File.ReadAllBytes("mksc.gba");
        Offsets offsets = new Offsets(file);
        Rom rom = new Rom(file, offsets);

        #region Init SDL
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0) Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        var window = SDL.SDL_CreateWindow("MkscEdit", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 1024, 1024, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
        
        if (window == IntPtr.Zero) Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        
        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        var renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        
        if (renderer == IntPtr.Zero) Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        
        var running = true;
        
        rom.ExtractTileGraphics();
        #endregion

        SDL_Rect elementPosition = new SDL_Rect() { x = 0, y = 0, w = 256, h = 256 };
        TilePanel tilePanel = new TilePanel(renderer,rom, elementPosition, new(0,0));
        tilePanel.SetTrack(Track.PeachCircuit);
         
        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        Console.WriteLine(tilePanel.GetTile(e.motion.x,e.motion.y));
                        break;
                }
            }
        
            // Sets the color that the screen will be cleared with.
            SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL_RenderClear(renderer);

            tilePanel.DrawElement();

            // Switches out the currently presented render surface with the one we just did work on.
            SDL_RenderPresent(renderer);
        }
        
        // Clean up the resources that were created.
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);
        SDL_Quit();
    }
}