using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.ResultModels
{
    public class Pagination
    {
        public int CurrentPage { get; internal set; }
        public int TotalPages { get; internal set; }
        public int PageSize { get; internal set; }
        public int TotalCount { get; internal set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
