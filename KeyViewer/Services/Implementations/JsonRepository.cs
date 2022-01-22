using System;
using System.Collections.Generic;
using System.IO;
using KeyViewer.Services.Abstraction;
using LogSystem;
using Newtonsoft.Json;

namespace KeyViewer.Services.Implementations
{
    public abstract class JsonRepository<T> : IRepository<T>
    {
        public event Action Update;

        public List<T> Repository { get; set; } = new List<T>();

        public string RepositoryName { get; protected set; }
        public string RepositoryDirectory { get; protected set; }
        protected string RepositoryPath => $"{RepositoryDirectory}\\{RepositoryName}";

        public JsonRepository(string RepositoryDirectory, string RepositoryName)
        {
            this.RepositoryName = RepositoryName;
            this.RepositoryDirectory = RepositoryDirectory;

            try
            {
                Load();
            }
            catch
            {
                InitFileSystem();
                Init();
            }
        }

        public abstract void Init();

        public void Load()
        {
            var loadRepository = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(RepositoryPath));

            if (loadRepository == null)
            {
                var backupPath = $"{RepositoryDirectory}\\Error_{DateTime.Now:MM.dd_hh:mm}_{RepositoryName}";
                File.Move(RepositoryPath, backupPath);
                Logger.Instance.WriteError($"Load failed. Backup created: {backupPath}");
                throw new Exception("Error when load");
            }

            Repository = loadRepository;
        }

        public void Save()
        {
            Update?.Invoke();
            try
            {
                File.WriteAllText(RepositoryPath, JsonConvert.SerializeObject(Repository, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteWarning($"Save failed. {ex.GetType()}: {ex.Message}");
            }
        }

        private void InitFileSystem()
        {
            if (!Directory.Exists(RepositoryDirectory))
            {
                Directory.CreateDirectory(RepositoryDirectory);
            }

            if (!File.Exists(RepositoryPath))
            {
                File.Create(RepositoryPath).Dispose();
            }
        }
    }
}
