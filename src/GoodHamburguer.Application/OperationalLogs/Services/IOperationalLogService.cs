using GoodHamburguer.Application.OperationalLogs.Contracts;

namespace GoodHamburguer.Application.OperationalLogs.Services;

public interface IOperationalLogService
{
    Task RecordAsync(OperationalLogRecordRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OperationalLogResponse>> ListAsync(OperationalLogQueryFilter filter, CancellationToken cancellationToken = default);
}
