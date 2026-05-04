namespace WebApplication1.DTOs.Response.Common
{
public record ApiResponse<T>(bool Success, string Message, T? Data, IEnumerable<string>? Errors = null);
 
public record PagedResponse<T>(
    IEnumerable<T> Data,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages);
}