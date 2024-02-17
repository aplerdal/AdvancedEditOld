using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
using AdvancedEdit.UI;
using SDL2;
using static SDL2.SDL;
using NativeFileDialog.Extended;
using System.Diagnostics;

namespace AdvancedEdit;
class Program{
    public static byte[] file = new byte[0];
    public static List<Track> tracks = new List<Track>();
    public static IntPtr Renderer = IntPtr.Zero;
    public static IntPtr Window = IntPtr.Zero;
    public static int WindowWidth, WindowHeight;

    public static Dictionary<int, bool> keyDown = new Dictionary<int, bool>();
    public static Dictionary<int, bool> keyPress = new Dictionary<int, bool>();
    public static Dictionary<int, bool> keyReleased = new Dictionary<int, bool>();
    static unsafe void Main(string[] args){

        #region Init SDL
        // Initilizes SDL.
        if (SDL_Init(SDL.SDL_INIT_VIDEO) < 0) Console.WriteLine($"There was an issue initilizing SDL. {SDL_GetError()}");

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        Window = SDL_CreateWindow("AdvancedEdit", SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 1024, 1024, SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_SHOWN);
         
        if (Window == IntPtr.Zero) Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
        
        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        Renderer = SDL_CreateRenderer(Window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        #endregion

        foreach (var key in Enum.GetValues(typeof(SDL_Keycode)))
        {
            keyDown.Add((int)key, false);
            keyPress.Add((int)key, false);
            keyReleased.Add((int)key, false);
        }

        // Open ROM
        while (true)
        {
            string str = NFD.OpenDialog("", new Dictionary<string, string>() { { "Game Boy Advance ROM", "gba" } });
            if (str != null)
            {
                if (Rom.OpenRom(str))
                {
                    Track.GenerateTracks();
                    break;
                }
                
            }
        }


        TrackEditor trackEditor = new TrackEditor();
        UIManager uiManager = new UIManager();

        // Main loop for the program
        var running = true;

        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            foreach (var i in keyReleased.Keys.ToList())
            {
                keyReleased[i] = false;
                keyPress[i] = false;
            }
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                uiManager.ElementEvents(e);
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        trackEditor.MouseUp(e);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        trackEditor.MouseDown(e);
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        trackEditor.ScrollWheel(e);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        trackEditor.MouseMotion(e);
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        if (keyDown[(int)e.key.keysym.sym] == false)
                        {
                            keyPress[(int)e.key.keysym.sym] = true;
                        }
                        keyDown[(int)e.key.keysym.sym] = true;
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        keyPress[(int)e.key.keysym.sym] = false;
                        keyReleased[(int)e.key.keysym.sym] = true;
                        keyDown[(int)e.key.keysym.sym] = false;
                        break;
                }
            }
            SDL_GetWindowSize(Window, out WindowWidth, out WindowHeight);
            trackEditor.Update();

            // Sets the color that the screen will be cleared with.
            SDL_SetRenderDrawColor(Renderer, 25, 25, 90, 255);
            SDL_RenderClear(Renderer);

            trackEditor.Draw();

            // Switches out the currently presented render surface with the one we just did work on.
            SDL_RenderPresent(Renderer);
        }
        //Test code
        trackEditor.Close();
        File.WriteAllBytes("MkscModified.gba", Track.CompileRom(tracks));
        
        // Clean up the resources that were created.
        SDL_DestroyRenderer(Renderer);
        SDL_DestroyWindow(Window);
        SDL_Quit();
    }
}