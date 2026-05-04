using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Models;
using WebApplication1.Interfaces.Common;

namespace  WebApplication1.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _uow;
    public InvoiceService(IUnitOfWork uow) => _uow = uow;

    public async Task<ApiResponse<InvoiceResponse>> GetByIdAsync(int id)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(id);
        if (invoice == null) return new ApiResponse<InvoiceResponse>(false, "Invoice not found.", null);
        return new ApiResponse<InvoiceResponse>(true, "Success.", MapResponse(invoice));
    }

    public async Task<ApiResponse<IEnumerable<InvoiceResponse>>> GetByCompanyAsync(int companyId)
    {
        var invoices = await _uow.Invoices.GetByCompanyAsync(companyId);
        return new ApiResponse<IEnumerable<InvoiceResponse>>(true, "Success.", invoices.Select(MapResponse));
    }

    public async Task<ApiResponse<InvoiceResponse>> CreateAsync(CreateInvoiceRequest request)
    {
        var company = await _uow.Companies.GetByIdAsync(request.CompanyId);
        if (company == null) return new ApiResponse<InvoiceResponse>(false, "Company not found.", null);

        var taxAmount = request.SubTotal * (request.TaxPercent / 100);
        var total = request.SubTotal + taxAmount - request.Discount;
        var invoiceNumber = await _uow.Invoices.GenerateInvoiceNumberAsync();

        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber,
            CompanyId = request.CompanyId,
            TradeId = request.TradeId,
            SubscriptionId = request.SubscriptionId,
            SubTotal = request.SubTotal,
            TaxPercent = request.TaxPercent,
            TaxAmount = taxAmount,
            Discount = request.Discount,
            TotalAmount = total,
            Currency = request.Currency,
            Status = InvoiceStatus.Draft,
            IssueDate = request.IssueDate,
            DueDate = request.DueDate,
            Notes = request.Notes
        };

        await _uow.Invoices.AddAsync(invoice);
        await _uow.SaveChangesAsync();

        invoice.Company = company;
        return new ApiResponse<InvoiceResponse>(true, "Invoice created successfully.", MapResponse(invoice));
    }

    public async Task<ApiResponse<InvoiceResponse>> UpdateStatusAsync(int id, UpdateInvoiceStatusRequest request)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(id);
        if (invoice == null) return new ApiResponse<InvoiceResponse>(false, "Invoice not found.", null);

        if (!Enum.TryParse<InvoiceStatus>(request.Status, true, out var status))
            return new ApiResponse<InvoiceResponse>(false, "Invalid status.", null);

        invoice.Status = status;
        invoice.UpdatedAt = DateTime.UtcNow;
        if (status == InvoiceStatus.Paid) invoice.PaidDate = DateTime.UtcNow;

        await _uow.Invoices.UpdateAsync(invoice);
        await _uow.SaveChangesAsync();

        return new ApiResponse<InvoiceResponse>(true, "Invoice status updated.", MapResponse(invoice));
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var exists = await _uow.Invoices.ExistsAsync(id);
        if (!exists) return new ApiResponse<string>(false, "Invoice not found.", null);
        await _uow.Invoices.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return new ApiResponse<string>(true, "Invoice deleted.", null);
    }

    private InvoiceResponse MapResponse(Invoice i) => new(
        i.Id, i.InvoiceNumber, i.CompanyId, i.Company?.Name ?? "",
        i.TradeId, i.Trade?.TradeNumber,
        i.SubscriptionId, i.SubTotal, i.TaxPercent,
        i.TaxAmount, i.Discount, i.TotalAmount,
        i.Currency, i.Status.ToString(),
        i.IssueDate, i.DueDate, i.PaidDate,
        i.Notes, i.CreatedAt
    );
}