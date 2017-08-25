using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MOE.Common.Models.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        Models.SPM db = new SPM();

        public List<Models.Comment> GetAll()
        {
            List<Models.Comment> comments = (from r in db.Comments
                                           select r).ToList();

            return comments;
        }

        //public List<Models.Comment> GetByEntityChartType(string Entity, int chartType, int entityType)
        //{
        //    List<Models.Comment> comments = (from r in db.Comment
        //                                     where r.EntityID == Entity
        //                                         && r.ChartType == chartType
        //                                         && r.EntityType == entityType
        //                                     select r).ToList();

        //    return comments;
        //}

       
    }

    
}
