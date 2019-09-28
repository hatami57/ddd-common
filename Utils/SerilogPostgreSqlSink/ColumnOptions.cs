using System.Collections.Generic;

namespace Serilog.Sinks.PostgreSql
{
    public class ColumnOptions
    {
        public static IDictionary<string, ColumnWriterBase> Default => new Dictionary<string, ColumnWriterBase>
        {
            {DefaultColumnNames.RenderedMesssage, new RenderedMessageColumnWriter()},
            {DefaultColumnNames.MessageTemplate, new MessageTemplateColumnWriter()},
            {DefaultColumnNames.Level, new LevelColumnWriter()},
            {DefaultColumnNames.Exception, new ExceptionColumnWriter()},
            {DefaultColumnNames.Properties, new PropertiesColumnWriter()},
            {DefaultColumnNames.CreatedAt, new TimestampColumnWriter()}
        };
    }

    public static class DefaultColumnNames
    {
        public const string RenderedMesssage = "message";
        public const string MessageTemplate = "message_template";
        public const string Level = "level";
        public const string Exception = "exception";
        public const string Properties = "properties";
        public const string CreatedAt = "created_at";
    }
}