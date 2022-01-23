using System;
using System.Collections.Generic;
using System.IO;
using EventHook.Abstractions;
using KeyViewer.Model.Programs;
using KeyViewer.Services.Implementations;
using LogSystem;
using Newtonsoft.Json;

namespace KeyViewer.Services
{
    public class ProgramsRepository : JsonRepository<ProgramSettings>
    {
        public static ProgramsRepository Instanse => _Instanse == null ? _Instanse = new ProgramsRepository("Programs.json") : _Instanse;
        private static ProgramsRepository _Instanse;

        public ProgramSettings GetDefault()
        {
            return new ProgramSettings
            {
                Alight = Default.Alight,
                AlightPosition = Default.AlightPosition,
                InGameAllTime = Default.InGameAllTime,
                DisplayNameSource = Default.DisplayNameSource,
                IsVisible = Default.IsVisible,
                TopmostTimer = Default.TopmostTimer,
                Window = Default.Window,
            };
        }

        public ProgramSettings Default => _Default == null ? _Default = LoadDefault() : _Default;
        private ProgramSettings _Default;
        private string DefaultPath => RepositoryDirectory + "\\DefaultProgram.json";

        private ProgramSettings LoadDefault()
        {
            try
            {
                var loadDefault = JsonConvert.DeserializeObject<ProgramSettings>(File.ReadAllText(DefaultPath));

                if (loadDefault == null)
                {
                    var backupPath = $"{RepositoryDirectory}\\Error_{DateTime.Now:MM.dd_hh:mm}_DefaultProgram.json";
                    File.Move(RepositoryPath, backupPath);
                    Logger.Instance.WriteError($"Load failed. Backup created: {backupPath}");
                    throw new Exception("Error when load");
                }

                return loadDefault;
            }
            catch
            {
                return new ProgramSettings { Window = new WindowParameters { FilePath = "Default", MainWindowTitle = "Default", ProcessName = "Defaut", ModuleName = "Default" } };
            }
        }


        private ProgramsRepository(string RepositoryName) : base("ProgramInfos", RepositoryName) { }

        public override void Init()
        {
            Repository = new List<ProgramSettings> { };
        }

        public new void Save()
        {
            base.Save();
            try
            {
                File.WriteAllText(DefaultPath, JsonConvert.SerializeObject(Default, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteWarning($"Save failed. {ex.GetType()}: {ex.Message}");
            }
        }
    }
}
