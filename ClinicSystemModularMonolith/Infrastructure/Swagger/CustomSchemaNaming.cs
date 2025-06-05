using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClinicSystemModularMonolith.Infrastructure.Swagger;

public static class CustomSchemaNaming
{
    public static string GetCustomSchemaId(Type type)
    {
        if (type.Namespace is null || !type.Namespace.Contains("Contracts"))
            return type.Name;

        // Берем последний сегмент пространства имён — например, "User"
        var parts = type.Namespace.Split('.');
        var contractType = parts.Last(); // Contracts.User → "User"

        return $"{contractType}_{type.Name}";
    }
}
