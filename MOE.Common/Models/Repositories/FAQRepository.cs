using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly SPM db = new SPM();

        public List<FAQ> GetAll()
        {
            return db.FAQs.OrderBy(f => f.OrderNumber).ToList();
        }

        public FAQ GetbyID(int id)
        {
            return db.FAQs.Where(m => m.FAQID == id).First();
        }

        public void Add(FAQ item)
        {
            db.FAQs.Add(item);
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            var faq = GetbyID(id);
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

        public void Update(FAQ item)
        {
            var faqFromDatabase = GetbyID(item.FAQID);
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