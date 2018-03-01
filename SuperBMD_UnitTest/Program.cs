﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperBMD;
using SuperBMD.Materials;

namespace SuperBMD_UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "./orima1.dae", "./olimar.bmd", "--mat", "./test2.json" };
            //args = new string[] { "C:/Users/User/Documents/3dsMax/export/testlevel.dae", "C:/Users/User/Documents/Sandbox/pack stuff/NeuPacker/arc/model.bmd", "--mat", "C:/Users/User/Documents/Sandbox/pack stuff/NeuPacker/MyMaterials/tutmat.json" };


            string in_file = "";
            string out_file = "";
            string mat_file = "";

            int processed_args = 0;

            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "--mat") {
                    mat_file = args[i + 1];
                    i++;
                }
                else if (args[i] == "help") {
                    DisplayHelp();
                    return;
                }
                else {
                    if (processed_args == 0) {
                        in_file = args[i];
                    }
                    else if (processed_args == 1) {
                        out_file = args[i];
                    }

                    processed_args++;
                }
            }

            if (in_file != "")
            {
                if (in_file.EndsWith(".bmd") || in_file.EndsWith(".bdl")) 
                {
                    Model mod = Model.Load(in_file);

                    string outFilepath;

                    if (out_file != "") {
                        outFilepath = out_file;
                    }
                    else {
                        string inDir = Path.GetDirectoryName(in_file);
                        string fileNameNoExt = Path.GetFileNameWithoutExtension(in_file);
                        outFilepath = Path.Combine(inDir, fileNameNoExt + ".dae");

                        if (mat_file == "") {
                            mat_file = Path.Combine(inDir, fileNameNoExt + "_mat.json");
                        }
                    }

                    mod.ExportAssImp(in_file, outFilepath, "dae", new ExportSettings());
                    if (mat_file != "") {
                        using (TextWriter file = File.CreateText(mat_file)) {
                            mod.Materials.DumpJson(file);
                        }
                    }
                }

                else {
                    List<Material> mat_presets = null;

                    if (mat_file != "") {
                        JsonSerializer serializer = new JsonSerializer();

                        using (TextReader file = File.OpenText(mat_file)) {
                            using (JsonTextReader reader = new JsonTextReader(file)) {
                                mat_presets = serializer.Deserialize<List<Material>>(reader);
                            }
                        }
                    }

                    Model mod = Model.Load(in_file, mat_presets);

                    if (out_file != "") {
                        mod.ExportBMD(out_file, true);
                    }
                    else {
                        mod.ExportBMD(in_file + ".bmd");
                    }

                    
                } 
            }
            else
                DisplayHelp();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("SuperBMD: A tool to import and export various 3D model formats into the Binary Model (BMD) format.");
            Console.WriteLine("Written by Sage_of_Mirrors/Gamma (@SageOfMirrors).");
            Console.WriteLine("Made possible with help from arookas, LordNed, xDaniel, and many others.");
            Console.WriteLine("Visit https://github.com/Sage-of-Mirrors/SuperBMD/wiki for more information.");
            Console.WriteLine("This is an inofficial version with a Triangle Strip algorithm added from BrawlLib. Report any issues to Yoshi2#6013 (RenolY2 on github).");
        }
    }
}
