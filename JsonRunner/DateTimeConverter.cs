
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonRunner;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string format = "dd.MM.yyyy";

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(format, CultureInfo.InvariantCulture));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string output = reader.GetString();

        if (DateTime.TryParseExact(output, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date;
        }
        else throw new JsonException("Не удалось получить дату");
    }
}

