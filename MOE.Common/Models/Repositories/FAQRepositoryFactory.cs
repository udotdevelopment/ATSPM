
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class FAQsRepositoryFactory
    {
        private static IFAQRepository faqRepository;

        public static IFAQRepository Create()
        {
            if (faqRepository != null)
            {
                return faqRepository;
            }
            return new FAQRepository();
        }

        public static void SetMenuRepository(IFAQRepository newRepository)
        {
            faqRepository = newRepository;
        }
    }
}
