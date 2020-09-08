// /*
//     Copyright (C) 2020  erri120
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <https://www.gnu.org/licenses/>.
// */

using System;
using System.IO;
using Newtonsoft.Json;

namespace MortalEnemies
{
    public static class Utils
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void LogThrow(Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }

        public static T FromJson<T>(string file)
        {
            var content = File.ReadAllText(file);
            var result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return result!;
        }
    }
}
