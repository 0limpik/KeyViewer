using System.Collections.Generic;

namespace KeyViewer.Services.Abstraction
{
    public interface IRepository<T>
    {
        List<T> Repository { get; }
        string RepositoryName { get; }
        string RepositoryDirectory { get; }

        void Init();
        void Load();
        void Save();
    }
}
