using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestERP.Application.Logging
{
    public static class RequestLogSerializer
    {
        private static readonly HashSet<string> SensitivePropertyNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "Password",
            "NewPassword",
            "OldPassword",
            "ConfirmPassword",
            "RefreshToken",
            "Token",
            "AccessToken",
            "Key"
        };

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        public static string? Serialize(object? value, int maxLength = 4000)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                var masked = MaskSensitiveValues(value);
                var json = JsonSerializer.Serialize(masked, JsonOptions);
                return Truncate(json, maxLength);
            }
            catch
            {
                return Truncate(value.GetType().Name, maxLength);
            }
        }

        private static object? MaskSensitiveValues(object value)
        {
            if (value is null)
            {
                return null;
            }

            var type = value.GetType();

            if (type.IsPrimitive || value is string or decimal or DateTime or DateTimeOffset or Guid)
            {
                return value;
            }

            if (type.IsEnum)
            {
                return value.ToString();
            }

            var result = new Dictionary<string, object?>();

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                object? propertyValue;
                try
                {
                    propertyValue = property.GetValue(value);
                }
                catch
                {
                    continue;
                }

                if (SensitivePropertyNames.Contains(property.Name))
                {
                    result[property.Name] = "***";
                    continue;
                }

                if (propertyValue is null)
                {
                    result[property.Name] = null;
                    continue;
                }

                var propertyType = propertyValue.GetType();
                if (propertyType.IsPrimitive || propertyValue is string or decimal or DateTime or DateTimeOffset or Guid)
                {
                    result[property.Name] = propertyValue;
                }
                else if (propertyType.IsEnum)
                {
                    result[property.Name] = propertyValue.ToString();
                }
                else
                {
                    result[property.Name] = MaskSensitiveValues(propertyValue);
                }
            }

            return result;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (value.Length <= maxLength)
            {
                return value;
            }

            return value[..maxLength];
        }
    }
}
