
namespace STS.DTOs.ResultModels
{
    public class PaginatedResult<TModel>
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public List<TModel> Items { get; private set; }
        public PaginatedResult(List<TModel> items, int count, int pageNumber, int pageSize)
        {
            Pagination.TotalCount = count;
            Pagination.PageSize = pageSize;
            Pagination.CurrentPage = pageNumber;
            Pagination.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
}
