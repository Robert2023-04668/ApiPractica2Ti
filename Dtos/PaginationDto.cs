namespace ApiPractica2Ti.Dtos
{
    public class PaginationDto<T>
    {
      
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Total { get; set; }
            public IEnumerable<T> Data { get; set; }
            public string search {  get; set; }
             
    }
}
