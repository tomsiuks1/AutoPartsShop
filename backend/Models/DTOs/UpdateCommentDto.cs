using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateCommentDto : CreateCommentDto
    {
        public Guid ProductId { get; set; }
        public Guid Id { get; set; }
    }
}