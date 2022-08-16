using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class FAQRepository : IFAQRepository
    {
        private readonly MOEContext db;

        public List<Faq> GetAll()
        {
            return db.Faqs.OrderBy(f => f.OrderNumber).ToList();
        }

        public Faq GetbyID(int id)
        {
            return db.Faqs.Where(m => m.Faqid == id).First();
        }

        public void Add(Faq item)
        {
            db.Faqs.Add(item);
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            var faq = GetbyID(id);
            if (faq != null)
            {
                db.Faqs.Remove(faq);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("FAQ Not Found");
            }
        }

        public void Update(Faq item)
        {
            var faqFromDatabase = GetbyID(item.Faqid);
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