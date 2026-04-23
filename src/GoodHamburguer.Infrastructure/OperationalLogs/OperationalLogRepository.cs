using GoodHamburguer.Application.OperationalLogs.Abstractions;
using GoodHamburguer.Application.OperationalLogs.Contracts;
using GoodHamburguer.Infrastructure.Persistence;
using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.OperationalLogs;

public sealed class OperationalLogRepository(GoodHamburguerDbContext dbContext) : IOperationalLogRepository
{
    public async Task AddAsync(OperationalLogRecordRequest request, CancellationToken cancellationToken = default)
    {
        dbContext.OperationalLogs.Add(new OperationalLogEntity
        {
            CreatedAtUtc = request.CreatedAtUtc,
            Type = ToStorageType(request.Type),
            Route = request.Route,
            Method = request.Method,
            CorrelationId = request.CorrelationId,
            Payload = string.IsNullOrWhiteSpace(request.Payload) ? "{}" : request.Payload,
            StatusCode = request.StatusCode,
            ExceptionType = request.ExceptionType,
            ErrorMessage = request.ErrorMessage
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OperationalLogResponse>> ListAsync(OperationalLogQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OperationalLogs
            .AsNoTracking()
            .AsQueryable();

        if (filter.Type is not null)
        {
            query = query.Where(log => log.Type == ToStorageType(filter.Type.Value));
        }

        if (filter.From is not null)
        {
            query = query.Where(log => log.CreatedAtUtc >= filter.From.Value);
        }

        if (filter.To is not null)
        {
            query = query.Where(log => log.CreatedAtUtc <= filter.To.Value);
        }

        var entities = await query
            .OrderByDescending(log => log.CreatedAtUtc)
            .Take(filter.Limit)
            .ToListAsync(cancellationToken);

        return entities
            .Select(log => new OperationalLogResponse
            {
                Id = log.Id,
                Type = log.Type,
                CreatedAtUtc = log.CreatedAtUtc,
                Route = log.Route,
                Method = log.Method,
                CorrelationId = log.CorrelationId,
                Payload = log.Payload,
                StatusCode = log.StatusCode,
                ExceptionType = log.ExceptionType,
                ErrorMessage = log.ErrorMessage
            })
            .ToArray();
    }

    private static string ToStorageType(OperationalLogType type)
    {
        return type switch
        {
            OperationalLogType.Application => "application",
            OperationalLogType.Error => "error",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported operational log type.")
        };
    }
}
