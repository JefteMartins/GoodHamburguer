using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GoodHamburguer.Application.Common.Telemetry;

public static class ApplicationTelemetry
{
    public const string ServiceName = "GoodHamburguer.Application";
    public const string ServiceVersion = "1.0.0";

    public static readonly ActivitySource ActivitySource = new(ServiceName, ServiceVersion);
    
    public static readonly Meter Meter = new(ServiceName, ServiceVersion);

    public static readonly Counter<long> OrdersCreatedCounter = Meter.CreateCounter<long>(
        "goodhamburguer.orders.created.count",
        unit: "{order}",
        description: "Number of orders created");

    public static readonly Counter<long> OrdersUpdatedCounter = Meter.CreateCounter<long>(
        "goodhamburguer.orders.updated.count",
        unit: "{order}",
        description: "Number of orders updated");

    public static readonly Counter<long> OrdersDeletedCounter = Meter.CreateCounter<long>(
        "goodhamburguer.orders.deleted.count",
        unit: "{order}",
        description: "Number of orders deleted");

    public static readonly Histogram<double> OrderProcessingDuration = Meter.CreateHistogram<double>(
        "goodhamburguer.orders.processing.duration",
        unit: "s",
        description: "Duration of order processing operations");
}
