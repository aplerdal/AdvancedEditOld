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

        //combine to images
        IntPtr tileAtlas = SDL_CreateRGBSurface(0, 8, 2048, 32, 0, 0, 0, 0);
        for (int i = 0; i < rom.tiles[(int)Track.PeachCircuit].Length; i++)
        {
            var t = rom.tiles[(int)Track.SkyGarden][i];
            IntPtr s = (IntPtr)t.ToImage();
            SDL_Rect d = new SDL_Rect { x = 0, y = i * 8, w = 8, h = 8 };
            if (SDL_BlitSurface(s, IntPtr.Zero, tileAtlas, ref d) < 0) { Console.WriteLine(SDL.SDL_GetError()); };
            SDL_FreeSurface(s);
        }

        SDL_Surface* tile = rom.tiles[(int)Track.PeachCircuit][0].ToImage();
        IntPtr texture = SDL_CreateTextureFromSurface(renderer,(IntPtr)tile);
        IntPtr textureAtlas = SDL_CreateTextureFromSurface(renderer, tileAtlas);
         
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
                }
            }
        
            // Sets the color that the screen will be cleared with.
            if (SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255) < 0) Console.WriteLine($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");
        
            // Clears the current render surface.
            if (SDL_RenderClear(renderer) < 0) Console.WriteLine($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");
            int rows = 16;
            int col = 256 / rows;
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = x * 8 + y * 8 * rows, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * 16, y = y * 16, w = 16, h = 16 };
                    //SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref d);
                    SDL_RenderCopy(renderer, textureAtlas, ref s, ref d);
                }
            }
            // Switches out the currently presented render surface with the one we just did work on.
            SDL_RenderPresent(renderer);
        }
        
        // Clean up the resources that were created.
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);
        SDL_Quit();
    }
}