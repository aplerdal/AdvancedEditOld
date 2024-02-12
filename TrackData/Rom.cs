using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MkscEdit.Compression;
using MkscEdit.Types;
using System.Diagnostics;

namespace MkscEdit.TrackData
{
    public static class Rom
    {
        public static unsafe bool OpenRom(string path)
        {
            var t = File.ReadAllBytes(path);
            if (!VerifyRom(t))
            {
                return false;
            }
            Program.file = t;
            return true;
        }

        public static bool VerifyRom(byte[] file)
        {
            //Check rom code   A                      M                      K                      E
            if ((file[0xac] == 0x41) & (file[0xad] == 0x4d) & (file[0xae] == 0x4b) & (file[0xaf] == 0x45))
            {
                return true;
            }
            return false;

        }
    }
}