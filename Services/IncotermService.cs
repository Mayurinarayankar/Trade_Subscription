using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Models;
using WebApplication1.Interfaces.Common;

namespace  WebApplication1.Services;
public class IncotermService : IIncotermService
{
    private readonly IUnitOfWork _uow;
    public IncotermService(IUnitOfWork uow) => _uow = uow;

    public async Task<ApiResponse<IEnumerable<IncotermResponse>>> GetAllAsync()
    {
        var items = await _uow.Incoterms.GetActiveAsync();
        return new ApiResponse<IEnumerable<IncotermResponse>>(true, "Success.", items.Select(Map));
    }

    public async Task<ApiResponse<IncotermResponse>> GetByIdAsync(int id)
    {
        var item = await _uow.Incoterms.GetByIdAsync(id);
        if (item == null) return new ApiResponse<IncotermResponse>(false, "Not found.", null);
        return new ApiResponse<IncotermResponse>(true, "Success.", Map(item));
    }

    public async Task<ApiResponse<IncotermResponse>> CreateAsync(CreateIncotermRequest request)
    {
        var incoterm = new Incoterm
        {
            Code = request.Code.ToUpper(),
            Name = request.Name,
            Description = request.Description,
            Type = (IncotermsType)request.Type
        };
        await _uow.Incoterms.AddAsync(incoterm);
        await _uow.SaveChangesAsync();
        return new ApiResponse<IncotermResponse>(true, "Incoterm created.", Map(incoterm));
    }

    private IncotermResponse Map(Incoterm i) =>
        new(i.Id, i.Code, i.Name, i.Description, i.Type.ToString(), i.IsActive);
}