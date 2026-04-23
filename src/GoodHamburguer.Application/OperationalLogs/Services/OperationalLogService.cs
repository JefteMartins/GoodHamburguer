using GoodHamburguer.Application.OperationalLogs.Abstractions;
using GoodHamburguer.Application.OperationalLogs.Contracts;

namespace GoodHamburguer.Application.OperationalLogs.Services;

public sealed class OperationalLogService(IOperationalLogRepository repository) : IOperationalLogService
{
    public Task RecordAsync(OperationalLogRecordRequest request, CancellationToken cancellationToken = default)
    {
        return repository.AddAsync(request, cancellationToken);
    }

    public Task<IReadOnlyList<OperationalLogResponse>> ListAsync(OperationalLogQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var safeLimit = filter.Limit switch
        {
            <= 0 => 100,
            > 500 => 500,
            _ => filter.Limit
        };

        return repository.ListAsync(
            new OperationalLogQueryFilter
            {
                Type = filter.Type,
                From = filter.From,
                To = filter.To,
                Limit = safeLimit
            },
            cancellationToken);
    }
}
