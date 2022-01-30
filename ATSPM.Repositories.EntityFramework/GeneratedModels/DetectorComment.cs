﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class DetectorComment
    {
        public int CommentId { get; set; }
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CommentText { get; set; }

        public virtual Detector IdNavigation { get; set; }
    }
}
