using System.Diagnostics;

namespace Webhook.Api.OpenTelemetry;

internal static class DiagnosticConfig
{
    internal static readonly ActivitySource ActivitySource = new("Webhook.Api");
}
