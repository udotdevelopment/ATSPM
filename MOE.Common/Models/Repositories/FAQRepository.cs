using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        Models.SPM db = new SPM();

        public List<Models.FAQ> GetAll()
        {
            return db.FAQs.OrderBy(f => f.OrderNumber).ToList();
        }

        public Models.FAQ GetbyID(int id)
        {
            return db.FAQs.Where(m => m.FAQID == id).First();
        }

        public void Add(Models.FAQ item)
        {
            db.FAQs.Add(item);
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            FAQ faq = GetbyID(id);
            if (faq != null)
            {
                db.FAQs.Remove(faq);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("FAQ Not Found");
            }
        }

        public void Update(Models.FAQ item)
        {
            FAQ faqFromDatabase = GetbyID(item.FAQID);
            if (faqFromDatabase != null)
            {
                db.Entry(faqFromDatabase).CurrentValues.SetValues(item);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("FAQ Not Found");
            }
        }

    }
}
