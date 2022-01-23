using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyViewer.Services.Implementations;

namespace KeyViewer.Services
{
    public class DeletedProgramsRepository : JsonRepository<string>
    {
        public static DeletedProgramsRepository Instanse => _Instanse == null ? _Instanse = new DeletedProgramsRepository("DeletedPrograms.json") : _Instanse;
        private static DeletedProgramsRepository _Instanse;
        private DeletedProgramsRepository(string RepositoryDirectory) : base("ProgramInfos", RepositoryDirectory) { }

        public override void Init()
        {
            Repository = new List<string>();
        }
    }
}
