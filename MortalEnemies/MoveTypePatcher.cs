using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace MortalEnemies
{
    public class MoveTypePatcher
    {
        const string MT_FILE = "move_types.json";

        private IPatcherState<ISkyrimMod, ISkyrimModGetter> state;
        private MortalEnemies.Settings settings;
        private List<FormKey> moveTypeKeys;
        private HashSet<FormKey> remixKeys; // Is a hash set overkill for 3 items =) ?


        public MoveTypePatcher(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, MortalEnemies.Settings settings)
        {
            this.state = state;
            this.settings = settings;
            // These formkeys editorids are in move_types.json
            this.moveTypeKeys = new List<FormKey>{
                Skyrim.MovementType.NPC_BowDrawn_MT,
                Skyrim.MovementType.NPC_Blocking_MT,
                Skyrim.MovementType.NPC_1HM_MT,
                Skyrim.MovementType.NPC_2HM_MT,
                Skyrim.MovementType.NPC_MagicCasting_MT,
                Skyrim.MovementType.NPC_Attacking_MT,
                Skyrim.MovementType.NPC_PowerAttacking_MT,
                Skyrim.MovementType.NPC_Attacking2H_MT
            };
            // These types have some records offset by +15 when rival remix setting is on.
            this.remixKeys = new HashSet<FormKey>
            {
                Skyrim.MovementType.NPC_Attacking2H_MT,
                Skyrim.MovementType.NPC_Attacking_MT,
                Skyrim.MovementType.NPC_PowerAttacking_MT
            };
        }

        
        public void run()
        {
            Dictionary<string, Dictionary<string, float>> mtData = loadMoveTypesFromFile();

            foreach (var movementTypeKey in this.moveTypeKeys)
            {

                if (!this.state.LinkCache.TryResolve<IMovementTypeGetter>(movementTypeKey, out var moveType) || moveType.EditorID == null)
                {
                    Console.Out.WriteLine($"Could not resolve form key for: {movementTypeKey.ID}");
                    continue;
                }
                if (!mtData.ContainsKey(moveType.EditorID))
                {
                    Console.Out.WriteLine($"No data in move_types.json for {moveType.EditorID}");
                    continue;
                }

                try
                {
                    var newMoveType = moveType.DeepCopy();
                    newMoveType.LeftWalk = mtData[moveType.EditorID]["Left Walk"];
                    newMoveType.LeftRun =  mtData[moveType.EditorID]["Left Run"];
                    newMoveType.RightWalk = mtData[moveType.EditorID]["Right Walk"];
                    newMoveType.RightRun = mtData[moveType.EditorID]["Right Run"];
                    newMoveType.ForwardWalk = mtData[moveType.EditorID]["Forward Walk"];
                    newMoveType.ForwardRun = mtData[moveType.EditorID]["Forward Run"];
                    newMoveType.BackWalk = mtData[moveType.EditorID]["Back Walk"];
                    newMoveType.BackRun = mtData[moveType.EditorID]["Back Run"];

                    newMoveType.RotateInPlaceWalk = 45;//mtData[moveType.EditorID]["Rotate in Place Walk"];
                    Console.WriteLine($"{moveType.EditorID} Rotate in Place Walk: {newMoveType.RotateInPlaceWalk}");

                    newMoveType.RotateInPlaceRun = 90.0f;// mtData[moveType.EditorID]["Rotate in Place Run"];
                    Console.WriteLine($"{moveType.EditorID} Rotate in Place Rim: {newMoveType.RotateInPlaceRun}");


                    if (mtData[moveType.EditorID].ContainsKey("Rotate while Moving Run")) // Not all entires have this defined
                    {
                        newMoveType.RotateWhileMovingRun = 120; // mtData[moveType.EditorID]["Rotate while Moving Run"];
                    } 
                    /*if (this.settings.CommitmentMode == AttackCommitment.RivalRemix && this.remixKeys.Contains(movementTypeKey))
                    {
                        newMoveType.RotateInPlaceWalk += 15.0000f;
                        newMoveType.RotateInPlaceRun += 15.0000f;
                        newMoveType.RotateWhileMovingRun += 15.0000f; // These records are guarenteed to pass the if block before this
                    } */
                    state.PatchMod.MovementTypes.Add(newMoveType);

                } catch(Exception e)
                {
                    Console.WriteLine($"Could not set data for {movementTypeKey}: {e.Message}");
                }
            }
        }


        private Dictionary<string, Dictionary<string, float>> loadMoveTypesFromFile()
        {
            string file = Path.Combine(this.state.ExtraSettingsDataPath, MT_FILE); // existence is checked when patcher is loaded
            
            var moveTypes = JObject.Parse(File.ReadAllText(file));
            if (moveTypes == null)
            {
                throw new Exception("Could not load movement types json file");
            }
            Dictionary<string, Dictionary<string, float>>? mtData = moveTypes.ToObject<Dictionary<string, Dictionary<string, float>>>();
            if (mtData == null)
            {
                throw new Exception("Could not parse movement types json file");
            }

            return mtData;
        }
    }
}
