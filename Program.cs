using MkscEdit.Extract;
using MkscEdit.Types;
using SDL2;
using static SDL2.SDL;
using System;
using System.Runtime.CompilerServices;
namespace MkscEdit;
class Program{
    static unsafe void Main(string[] args){
        byte[] file = File.ReadAllBytes("/home/antimattur/Downloads/mksc.gba");
        Offsets offsets = new Offsets(file);
        Rom rom = new Rom(file, offsets);

        // Initilizes SDL.
        if (SDL_Init(SDL.SDL_INIT_VIDEO) < 0) Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        var window = SDL_CreateWindow("MkscEdit", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
        
        if (window == IntPtr.Zero) Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        
        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        var renderer = SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        
        if (renderer == IntPtr.Zero) Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        
        var running = true;
        
        rom.ExtractTileGraphics();

        //combine to images
        foreach (var t in rom.tiles[(int)Track.PeachCircuit]){
            SDL.SDL_Surface* s = t.ToImage();
            SDL_FreeSurface((IntPtr)s);
        }
        SDL.SDL_Surface* tile = rom.tiles[(int)Track.PeachCircuit][0].ToImage();
        IntPtr texture = SDL.SDL_CreateTextureFromSurface(renderer,(IntPtr)tile);
        
        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
        
            // Sets the color that the screen will be cleared with.
            if (SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255) < 0)
            {
                Console.WriteLine($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");
            }
        
            // Clears the current render surface.
            if (SDL.SDL_RenderClear(renderer) < 0)
            {
                Console.WriteLine($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");
            }
            SDL.SDL_Rect r = new SDL.SDL_Rect() {x=0,y=0,w=8,h=8};
            SDL.SDL_Rect s = new SDL.SDL_Rect() {x=0,y=0,w=64,h=64};
            SDL.SDL_RenderCopy(renderer, texture, ref r, ref s);
        
            // Switches out the currently presented render surface with the one we just did work on.
            SDL.SDL_RenderPresent(renderer);
        }
        
        // Clean up the resources that were created.
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }
}