using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class CommentRepositoryFactory
    {
        private static ICommentRepository commentsRepository;

        public static ICommentRepository CreateSPMCommentRepository()
        {
            if (commentsRepository != null)
            {
                return commentsRepository;
            }
            return new CommentRepository();
        }

        public static void SetSPMCommentRepository(ICommentRepository newRepository)
        {
            commentsRepository = newRepository;
        }
    }
}
