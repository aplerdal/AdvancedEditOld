using MkscEdit.Extract;
using MkscEdit.Types;
using MkscEdit.UI;
using SDL2;
using NativeFileDialog.Extended;
using static SDL2.SDL;
using static SDL2.SDL_ttf;
using System.Diagnostics;

namespace MkscEdit;
class Program{
    public static IntPtr Sans;
    public static byte[] file;
    public static Offsets offsets;
    public static Rom rom;
    public static IntPtr Renderer;
    public static IntPtr Window;
    public static int WindowWidth, WindowHeight;
    static unsafe void Main(string[] args){
        #region Init SDL
        // Initilizes SDL.
        if (SDL_Init(SDL.SDL_INIT_VIDEO) < 0) Console.WriteLine($"There was an issue initilizing SDL. {SDL_GetError()}");

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        var window = SDL_CreateWindow("MkscEdit", SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 1024, 1024, SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_SHOWN);
         
        if (window == IntPtr.Zero) Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
        
        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        Renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        
        if (Renderer == IntPtr.Zero) Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        TTF_Init();

        Sans = TTF_OpenFont("arial.ttf", 12);
        if (Sans == IntPtr.Zero) { throw new Exception(SDL_GetError()); }

        var running = true;
        #endregion

        SDL_GetWindowSize(window, out WindowWidth, out WindowHeight);

        while (true)
        {
            string str = NFD.OpenDialog("", new Dictionary<string, string>() { { "Game Boy Advance ROM", "gba" } });
            if (str != null)
            {
                rom = new Rom();
                if (rom.OpenRom(str))
                {
                    offsets = new Offsets(file);
                    rom.ExtractTileGraphics();
                    break;
                }
                
            }
        }
        TrackEditor trackEditor = new TrackEditor();

        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        trackEditor.MouseDown(e);
                        break;
                }
            }

            trackEditor.Update();
        
            // Sets the color that the screen will be cleared with.
            SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
            SDL_RenderClear(Renderer);

            trackEditor.Draw();

            // Switches out the currently presented render surface with the one we just did work on.
            SDL_RenderPresent(Renderer);
        }
        
        // Clean up the resources that were created.
        SDL_DestroyRenderer(Renderer);
        SDL_DestroyWindow(window);
        SDL_Quit();
    }
}