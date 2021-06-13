
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using NitroSharp.Formats.ROM;
// using NitroSharp.IO;
// 
// namespace SwissArmyKnife.Patching
// {
//     class ROMTool
//     {
//         ROM Parser;
//         string DataPath;
//         public ROMTool(string Path, string DataPath)
//         {
//             Parser = new ROM(Path);
//             this.DataPath = DataPath;
//         }
//         public NitroFile GetFileFromOriginROM(string Path) => NitroDirectory.SearchDirectoryForFile(Parser.Root, Path, "/");
// 
//         public void PatchNARC(string Path)
//         {
//             void ModifyNARCFiles(NitroFile NARC)
//             {
//                 string TempPath = Path.Replace("/", "");
//                 new BinaryWriter(File.OpenWrite(TempPath)).Write(NARC.FileData);
//                 Util.ExtractNARC(TempPath);
//                 Util.MakeNARC(TempPath);
//                 NARC.FileData = File.ReadAllBytes(TempPath);
//             }
//             AbstractPatchFile(Path, ModifyNARCFiles);
//         }
// 
//         public void PatchFile(string Path)
//         {
//             void ReplaceFile(NitroFile NARC) => NARC.FileData = File.ReadAllBytes($"{DataPath}/{Path}");
//             AbstractPatchFile(Path, ReplaceFile);
//         }
// 
//         private void AbstractPatchFile(string Path, Action<NitroFile> Methodology)
//         {
//             NitroFile Original = GetFileFromOriginROM(Path);
//             if (Original != null)
//                 Methodology(Original);
// 
//             throw new Exception("Adding files is not yet supported.");
//         }
//     }
// }
