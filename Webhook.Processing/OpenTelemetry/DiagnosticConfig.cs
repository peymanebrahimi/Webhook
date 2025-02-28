using System.Diagnostics;

namespace Webhook.Processing.OpenTelemetry;

internal static class DiagnosticConfig
{
    internal static readonly ActivitySource ActivitySource = new("Webhook.Processing");
}
