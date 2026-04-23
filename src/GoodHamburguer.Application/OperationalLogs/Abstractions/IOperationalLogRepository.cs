using GoodHamburguer.Application.OperationalLogs.Contracts;

namespace GoodHamburguer.Application.OperationalLogs.Abstractions;

public interface IOperationalLogRepository
{
    Task AddAsync(OperationalLogRecordRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OperationalLogResponse>> ListAsync(OperationalLogQueryFilter filter, CancellationToken cancellationToken = default);
}
