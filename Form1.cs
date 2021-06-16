using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeaterLibrary.Formats.Scripts;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.Formats.Furniture;
using BeaterLibrary;

namespace SwissArmyKnife
{
    public partial class Form1 : Form 
    {
        MapContainer CurrentContainer;
        WBOverworldEntity Overworld;
        string Script_SelectedGame;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentContainer = new MapContainer();
            Overworld = new WBOverworldEntity();
        }

        private string DoFileDialog(FileDialog dialog, string[] filter)
        {
            dialog.Filter = string.Join('|', filter);
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() is DialogResult.OK)
                return dialog.FileName;
            else
                return "";
        }
        private string DoOpenFileDialog(params string[] filter) => DoFileDialog(new OpenFileDialog(), filter);
        private string DoSaveFileDialog(params string[] filter) => DoFileDialog(new SaveFileDialog(), filter);

        private void TabToOpenMap(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    ScriptContainer(false);
                    break;
                case 1:
                    TextBank(false);
                    break;
                case 2:
                    MapContainer(false);
                    break;
                case 3:
                    OverworldContainer(false);
                    break;
                default:
                    break;
            }
        }

        private void TabToSaveMap(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    ScriptContainer(true);
                    break;
                case 1:
                    TextBank(true);
                    break;
                case 2:
                    MapContainer(true);
                    break;
                case 3:
                    OverworldContainer(true);
                    break;
                default:
                    break;
            }
        }

        private void AbstractMenuStripChoiceHandler(bool save, Action<string> SaveMethod, Action<string> OpenMethod, params string[] filter)
        {
            string path = save ? DoSaveFileDialog(filter) : DoOpenFileDialog(filter);
            if (path.Equals(""))
                return;

            try
            {
                if (save)
                    SaveMethod(path);
                else
                    OpenMethod(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScriptContainer(bool save)
        {
            void ScriptToBinary(string path)
            {
                int[][] SelectedPlugins = new int[][] { };
                using (Forms.GameSelect gs = new Forms.GameSelect())
                {
                    if (gs.ShowDialog() is DialogResult.OK)
                    {
                        Script_SelectedGame = gs.selected;
                        if (gs.selected.Equals("B2W2"))
                        {
                            Forms.ScriptPluginSelect plug = new Forms.ScriptPluginSelect();
                            plug.ShowDialog();
                            SelectedPlugins = plug.SelectedPlugins;
                        }
                    }
                    else
                        return;
                }
                ScriptToAssembler("temp.s", SelectedPlugins);
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "arm-none-eabi-as.exe",
                        Arguments = $"-mthumb -c temp.s -o temp.o",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                proc?.Start();
                proc?.WaitForExit();

                var output = proc.StandardError.ReadToEnd();
                if (output.Length > 0)
                {
                    MessageBox.Show(output, "Error when writing script.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    
                proc = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "arm-none-eabi-objcopy.exe",
                        Arguments = $"-O binary temp.o \"{Path.GetFullPath(path)}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                proc?.Start();
                proc?.WaitForExit();

                output = proc.StandardError.ReadToEnd();
                if (output.Length > 0)
                {
                    MessageBox.Show(output, "Error when writing script.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cleanup.
                File.Delete("temp.o");
                File.Delete("temp.s");
            }

            void BinaryToScript(string path)
            {
                Script p;
                using (Forms.GameSelect gs = new Forms.GameSelect())
                {
                    if (gs.ShowDialog() is DialogResult.OK)
                    {
                        int[][] SelectedPlugins = new int[][] { };
                        Script_SelectedGame = gs.selected;
                        if (gs.selected.Equals("B2W2"))
                        {
                            Forms.ScriptPluginSelect plug = new Forms.ScriptPluginSelect();
                            plug.ShowDialog();
                            SelectedPlugins = plug.SelectedPlugins;
                        }
                        p = new Script(path, gs.selected, SelectedPlugins);
                    }
                    else
                        return;
                }

                ScriptEditorTextBox.Text = string.Join('\n', BeaterLibrary.Util.Flatten(p.Scripts.Values.ToList(), "Script_", true), BeaterLibrary.Util.Flatten(p.Functions.Values.ToList(), "FunctionLabel Function", false), BeaterLibrary.Util.Flatten(p.Movements.Values.ToList(), "MovementLabel Movement", false));
            }

            AbstractMenuStripChoiceHandler(save, ScriptToBinary, BinaryToScript, "Script Files (*.bin)|*.bin");
        }

        private void TextBank(bool save)
        {
            void BinaryToText(string path) => TextEditorTextBox.Text = new TextContainer().ParseText(Path.GetFullPath(path));
            void TextToBinary(string path) => TextContainer.Serialize(TextEditorTextBox.Text, Path.GetFullPath(path));

            try
            {
                AbstractMenuStripChoiceHandler(save, TextToBinary, BinaryToText, "Text Files (*.bin)|*.bin");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, save ? "Text compilation error." : "Text parsing error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MapContainer(bool save)
        {
            void BinaryToMapContainer(string path)
            {
                MapContainer newContainer = new MapContainer();
                using (BinaryReader b = new BinaryReader(File.Open(Path.GetFullPath(path), FileMode.Open)))
                {
                    uint magic = b.ReadUInt32(), modelAddress = b.ReadUInt32(), permAddress = b.ReadUInt32(), bldAddress = b.ReadUInt32(), fileSize = b.ReadUInt32();
                    newContainer.Model = b.ReadBytes((int)(permAddress - modelAddress)).ToList();
                    newContainer.Permissions = b.ReadBytes((int)(bldAddress - permAddress)).ToList();
                    newContainer.BuildingPositions = b.ReadBytes((int)(fileSize - bldAddress)).ToList();
                }
                CurrentContainer = newContainer;
            }

            void MapContainerToBinary(string path) => CurrentContainer.Serialize(Path.GetFullPath(path));

            AbstractMenuStripChoiceHandler(save, MapContainerToBinary, BinaryToMapContainer, "Map Container (*.bin)|*.bin");
        }

        private void OverworldContainer(bool save)
        {
            void BinaryToOverworldContainer(string path)
            {
                Overworld = new WBOverworldEntity(path);
                // Populate data.
                InteractableGridView.Rows.Clear();
                NPCGridView.Rows.Clear();
                WarpGridView.Rows.Clear();
                TriggerGridView.Rows.Clear();
                LevelScriptGridView.Rows.Clear();

                Overworld.Interactables.ForEach(x => InteractableGridView.Rows.Add(x.Script, x.Condition, x.Interactibility, x.RailIndex, x.X, x.Y, x.Z));
                Overworld.NPCs.ForEach(x => NPCGridView.Rows.Add(x.ID, x.ModelID, x.MovementPermission, x.Type, x.SpawnFlag, x.ScriptID, x.FaceDirection, x.SightRange, x.Unknown, x.Unknown2, 
                    x.TraversalWidth, x.TraversalHeight, x.StartingX, x.StartingY, x.X, x.Y, x.Unknown3, x.Z));
                Overworld.Warps.ForEach(x => WarpGridView.Rows.Add(x.TargetZone, x.TargetWarp, x.ContactDirection, x.TransitionType, x.CoordinateType, x.X, x.Z, x.Y, x.W, x.H, x.Rail));
                Overworld.Triggers.ForEach(x => TriggerGridView.Rows.Add(x.Script, x.ValueNeededForExecution, x.Variable, x.Unknown, x.Unknown2, x.X, x.Y, x.W, x.H, x.Z, x.Unknown3));
                Overworld.LevelScripts.ForEach(x => LevelScriptGridView.Rows.Add(x.Unknown, x.Unknown2, x.Unknown3, x.Data != null ? x.Data.Unknown : -1, x.Data != null ? x.Data.Unknown2 : -1, x.Data != null ? x.Data.Unknown3 : -1));
            }
            void OverworldContainerToBinary(string path)
            {
                List<Interactable> NewInteractables = new List<Interactable>();
                List<NPC> NewNPCs = new List<NPC>();
                List<Warp> NewWarps = new List<Warp>();
                List<Trigger> NewTriggers = new List<Trigger>();
                List<LevelScriptDeclaration> NewLevelScripts = new List<LevelScriptDeclaration>();

                // TODO: There has to be a better way to do this. But this works, for now.
                for (int i = 0; i < InteractableGridView.Rows.Count - 1; ++i)
                {
                    int index = 0;
                    if (InteractableGridView.Rows[i].Cells.Contains(null))
                        throw new Exception($"\"Interactables, Row {i}\" contains one or more empty cells. Fill out the attributes and try again.");
                    
                    NewInteractables.Add(new Interactable()
                    {
                        Script = (ushort)InteractableGridView.Rows[i].Cells[index++].Value,
                        Condition = (ushort)InteractableGridView.Rows[i].Cells[index++].Value,
                        Interactibility = (ushort)InteractableGridView.Rows[i].Cells[index++].Value,
                        RailIndex = (ushort)InteractableGridView.Rows[i].Cells[index++].Value,
                        X = (uint)InteractableGridView.Rows[i].Cells[index++].Value,
                        Y = (uint)InteractableGridView.Rows[i].Cells[index++].Value,
                        Z = (int)InteractableGridView.Rows[i].Cells[index].Value,
                    });
                }

                for (int i = 0; i < NPCGridView.Rows.Count - 1; ++i)
                {
                    int index = 0;
                    if (NPCGridView.Rows[i].Cells.Contains(null))
                        throw new Exception($"\"NPCs, Row {i}\" contains one or more empty cells. Fill out the attributes and try again.");
                    NewNPCs.Add(new NPC()
                    {
                        ID = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        ModelID = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        MovementPermission = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Type = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        SpawnFlag = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        ScriptID = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        FaceDirection = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        SightRange = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Unknown = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Unknown2 = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        TraversalHeight = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        TraversalWidth = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        StartingX = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        StartingY = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        X = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Y = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Unknown3 = (ushort)NPCGridView.Rows[i].Cells[index++].Value,
                        Z = (short)NPCGridView.Rows[i].Cells[index].Value,
                    });
                }

                for (int i = 0; i < WarpGridView.Rows.Count - 1; ++i)
                {
                    int index = 0;
                    if (WarpGridView.Rows[i].Cells.Contains(null))
                        throw new Exception($"\"Warps, Row {i}\" contains one or more empty cells. Fill out the attributes and try again.");
                    NewWarps.Add(new Warp()
                    {
                        TargetZone = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        TargetWarp = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        ContactDirection = (byte)WarpGridView.Rows[i].Cells[index++].Value,
                        TransitionType = (byte)WarpGridView.Rows[i].Cells[index++].Value,
                        CoordinateType = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        X = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        Z = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        Y = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        W = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        H = (ushort)WarpGridView.Rows[i].Cells[index++].Value,
                        Rail = (ushort)WarpGridView.Rows[i].Cells[index].Value,
                    });
                }

                for (int i = 0; i < TriggerGridView.Rows.Count - 1; ++i)
                {
                    int index = 0;
                    if (TriggerGridView.Rows[i].Cells.Contains(null))
                        throw new Exception($"\"Triggers, Row {i}\" contains one or more empty cells. Fill out the attributes and try again.");
                    NewTriggers.Add(new Trigger()
                    {
                        Script = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        ValueNeededForExecution = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Variable = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Unknown = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Unknown2 = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        X = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Y = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        W = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        H = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Z = (ushort)TriggerGridView.Rows[i].Cells[index++].Value,
                        Unknown3 = (ushort)TriggerGridView.Rows[i].Cells[index].Value,
                    });
                }

                for (int i = 0; i < LevelScriptGridView.Rows.Count - 1; ++i)
                {
                    int index = 0;
                    bool isInvalid = false;
                    if (LevelScriptGridView.Rows[i].Cells.Contains(null))
                        throw new Exception($"\"Level Scripts, Row {i}\" contains one or more empty cells. Fill out the attributes and try again.");
                    NewLevelScripts.Add(new LevelScriptDeclaration()
                    {
                        Unknown = (ushort)LevelScriptGridView.Rows[i].Cells[index++].Value,
                        Unknown2 = (ushort)LevelScriptGridView.Rows[i].Cells[index++].Value,
                        Unknown3 = (ushort)LevelScriptGridView.Rows[i].Cells[index++].Value,
                    });

                    for (int j = index; j < LevelScriptGridView.Rows[i].Cells.Count && !isInvalid; ++j)
                        isInvalid = LevelScriptGridView.Rows[i].Cells[j].Value == null || ((short)LevelScriptGridView.Rows[i].Cells[j].Value).Equals(-1);

                    NewLevelScripts[^1].Data = isInvalid ? null : new LevelScriptData()
                    {
                        Unknown = Convert.ToUInt16(LevelScriptGridView.Rows[i].Cells[index++].Value),
                        Unknown2 = Convert.ToUInt16(LevelScriptGridView.Rows[i].Cells[index++].Value),
                        Unknown3 = Convert.ToUInt16(LevelScriptGridView.Rows[i].Cells[index].Value),
                    };
                }

                // Now that the mess above is over...
                Overworld.Interactables = NewInteractables;
                Overworld.NPCs = NewNPCs;
                Overworld.Triggers = NewTriggers;
                Overworld.Warps = NewWarps;
                Overworld.LevelScripts = NewLevelScripts;

                Overworld.Serialize(path);
            }
            
            AbstractMenuStripChoiceHandler(save, OverworldContainerToBinary, BinaryToOverworldContainer, "Overworld Container (*.bin)|*.bin");
        }

        private void ScriptToAssembler(string path, int[][] ScriptPlugins)
        {
            StringBuilder s = new StringBuilder();
            BeaterLibrary.Util.GenerateCommandASM(Script_SelectedGame, ScriptPlugins);
            s.Append($".include \"{Script_SelectedGame}.s\"{Environment.NewLine}");
            s.Append($"Header:{Environment.NewLine}");
            Regex r = new Regex(@".*Script_\w+:.*");
            foreach (Match m in r.Matches(ScriptEditorTextBox.Text))
                s.Append($"\tscript {m.Value.Split(':')[0]}{Environment.NewLine}");
            s.Append($"EndHeader{Environment.NewLine}");

            using (StreamWriter sw = new StreamWriter(path))
            {
                string output = string.Join(Environment.NewLine, s.ToString(), ScriptEditorTextBox.Text);
                sw.Write(output);
            }
        }

        private void ImportMapContainerModel(object sender, EventArgs e)
        {
            string path = DoOpenFileDialog("Model (*.nsbmd)|*.nsbmd", "Model (*.bmd0)|*.bmd0");

            if (path.Equals(""))
                return;

            CurrentContainer.Model = File.ReadAllBytes(Path.GetFullPath(path)).ToList();
        }

        private void ExportMapContainerModel(object sender, EventArgs e)
        {
            string path = DoSaveFileDialog("Model (*.nsbmd)|*.nsbmd", "Model (*.bmd0)|*.bmd0");

            if (path.Equals(""))
                return;

            using (BinaryWriter b = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                b.Write(CurrentContainer.Model.ToArray());
        }

        private void ImportMapContainerPermissions(object sender, EventArgs e)
        {
            string path = DoOpenFileDialog("Permissions (*.per)|*.per");

            if (path.Equals(""))
                return;

            CurrentContainer.Permissions = File.ReadAllBytes(path).ToList();
        }

        private void ExportMapContainerPermissions(object sender, EventArgs e)
        {
            string path = DoSaveFileDialog("Permissions (*.per)|*.per");

            if (path.Equals(""))
                return;

            using (BinaryWriter b = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                b.Write(CurrentContainer.Permissions.ToArray());
        }

        private void ImportMapContainerBuildingPositions(object sender, EventArgs e)
        {
            string path = DoOpenFileDialog("Building Positions (*.bld)|*.bld");

            if (path.Equals(""))
                return;

            CurrentContainer.BuildingPositions = File.ReadAllBytes(path).ToList();
        }

        private void ExportMapContainerBuildingPositions(object sender, EventArgs e)
        {
            string path = DoSaveFileDialog("Building Positions (*.bld)|*.bld");

            if (path.Equals(""))
                return;

            using (BinaryWriter b = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                b.Write(CurrentContainer.BuildingPositions.ToArray());
        }

        private void ExportMapContainerToBinary(object sender, EventArgs e)
        {
            string path = DoSaveFileDialog("Map Container (*.bin)|*.bin");

            if (path.Equals(""))
                return;

            CurrentContainer.Serialize(path);
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }
    }
}
