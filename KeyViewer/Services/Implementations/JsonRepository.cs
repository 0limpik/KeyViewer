using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KeyViewer.Services.Abstraction;
using Newtonsoft.Json;

namespace KeyViewer.Services.Implementations
{
    public abstract class JsonRepository<T> : IRepository<T>
    {
        public List<T> Repository = new List<T>();

        public string RepositoryName { get; protected set; }
        public string RepositoryDirectory { get; protected set; }
        protected string RepositoryPath => $"{RepositoryDirectory}\\{RepositoryName}";

        public JsonRepository(string RepositoryDirectory, string RepositoryName)
        {
            this.RepositoryName = RepositoryName;
            this.RepositoryDirectory = RepositoryDirectory;

            Load();

            if (InDesignMode()) return;

            InitAutoSave();
        }

        async void InitAutoSave()
        {
            while (true)
            {
                await Task.Delay(5000);
                Save();
            }
        }

        public bool AddOrUpdate(Func<T, bool> predicate, T entity)
        {
            if (!Repository.Any(predicate))
            {
                Repository.Add(entity);
                return true;
            }
            else
            {
                var value = Repository.First(predicate);

                value = entity;

                return false;
            }
        }

        public void Add(T entity)
        {
            Repository.Add(entity);
            Save();
        }

        public abstract void Init();

        public void Load()
        {
            if (InDesignMode())
            {
                Init();
                return;
            }

            Directory.CreateDirectory(RepositoryDirectory);

            if (File.Exists(RepositoryPath))
            {
                try
                {
                    var loadRepository = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(RepositoryPath));
                    Repository = loadRepository ?? throw new Exception("Ошибка при загрузке");
                    return;
                }
                catch (Exception ex)
                {
                    File.Move(RepositoryPath, $"{RepositoryDirectory}\\Error_{RepositoryName}");
                }
            }
            else
            {
                File.Create(RepositoryPath).Dispose();
            }

            Init();

            Save();
        }

        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(RepositoryPath, JsonConvert.SerializeObject(Repository, Formatting.Indented));
            }
            catch
            {

            }
        }

        private async void FileChange(object sender, FileSystemEventArgs e)
        {
            await Task.Delay(100);
            Load();
        }
    }
}
