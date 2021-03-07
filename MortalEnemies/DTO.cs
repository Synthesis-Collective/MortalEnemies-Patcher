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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace MortalEnemies
{
    public class Config
    {
        [JsonProperty("classifications")]
        public Dictionary<string, List<string>> Classifications { get; set; } = new Dictionary<string, List<string>>();
        [JsonProperty("attackData")]
        public Dictionary<string, AttackData> AttackData { get; set; } = new Dictionary<string, AttackData>();
    }

    public class AttackData
    {
        [JsonProperty("Unarmed Reach")] public float UnarmedReach { get; set; } = float.MaxValue;
        [JsonProperty("Angular Acceleration")] public float AngularAcceleration { get; set; } = float.MaxValue;
        [JsonProperty("Angle Tolerance")] public float AngleTolerance { get; set; } = float.MaxValue;
        
        [JsonProperty("Attacks")]
        public Dictionary<string, Attack> Attacks { get; set; } = new Dictionary<string, Attack>();
    }

    public class Attack
    {
        [JsonProperty("Strike Angle")] public float StrikeAngle { get; set; } = float.MaxValue;
        [JsonProperty("Attack Angle")] public float AttackAngle { get; set; } = float.MaxValue;
    }
}
