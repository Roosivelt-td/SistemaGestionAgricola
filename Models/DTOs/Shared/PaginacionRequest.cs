namespace SistemaGestionAgricola.Models.DTOs.Shared
{
    public class PaginacionRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
        public string Search { get; set; } = string.Empty;
    }
}