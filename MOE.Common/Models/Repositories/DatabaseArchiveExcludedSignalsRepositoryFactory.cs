using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DatabaseArchiveExcludedSignalsRepositoryFactory
    {
        private static IDatabaseArchiveExcludedSignalsRepository databaseArchiveExcludedSignalsRepository;

        public static IDatabaseArchiveExcludedSignalsRepository Create()
        {
            if (databaseArchiveExcludedSignalsRepository != null)
            {
                return databaseArchiveExcludedSignalsRepository;
            }
            return new DatabaseArchiveExcludedSignalsRepository();
        }

        public static IDatabaseArchiveExcludedSignalsRepository Create(SPM context)
        {
            if (databaseArchiveExcludedSignalsRepository != null)
            {
                return databaseArchiveExcludedSignalsRepository;
            }
            return new DatabaseArchiveExcludedSignalsRepository(context);
        }

        public static void SetDatabaseArchiveExcludedSignalsRepository(IDatabaseArchiveExcludedSignalsRepository newRepository)
        {
            databaseArchiveExcludedSignalsRepository = newRepository;
        }
    }
}
