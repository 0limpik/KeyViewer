using System.Collections.Generic;
using KeyViewer.Model.Programs;
using KeyViewer.Services.Implementations;
using KeyViewer.View;

namespace KeyViewer.Services
{
    public class ProgramsRepository : JsonRepository<ProgramSettings>
    {
        public static ProgramsRepository Instanse => _Instanse == null ? _Instanse = new ProgramsRepository("Programs.json") : _Instanse;
        private static ProgramsRepository _Instanse;

        private ProgramsRepository(string RepositoryName) : base("ProgrammInfos", RepositoryName) { }

        public override void Init()
        {
            Repository = new List<ProgramSettings> { };
        }
    }
}
