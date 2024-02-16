using static SDL2.SDL;
namespace AdvancedEdit.Types{
    public static class ExtensionMethods{
        public static bool Contains(this SDL_Rect rect, int x, int y){
            if ( x > rect.x && y > rect.y && x < rect.x+rect.w && y < rect.y+rect.h) return true;
            return false;
        }
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (-1 == j) ? Arr[Arr.Length-1] : Arr[j];
        }
    }

}