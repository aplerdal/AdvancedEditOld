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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AdvancedEdit.Compression;
using AdvancedEdit.Types;
using System.Diagnostics;

namespace AdvancedEdit.TrackData
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
            AdvancedEditor.file = t;
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