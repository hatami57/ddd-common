using System;
using System.Collections.Generic;
using System.Text;

namespace DDDCommon.Utils
{
    public static class Errors
    {
        public static Error InternalError(string details = null) =>
            new Error { Code = 1, Text = "internal_error", Details = details };
        public static Error NotFound(string details = null) =>
            new Error { Code = 2, Text = "not_found", Details = details };
        public static Error AlreadyDone(string details = null) =>
            new Error { Code = 3, Text = "already_done", Details = details };
        public static Error InvalidOperation(string details = null) =>
            new Error { Code = 4, Text = "invalid_operation", Details = details };
        public static Error DatabaseError(string details = null) =>
            new Error { Code = 5, Text = "database_error", Details = details };
        public static Error DuplicateKey(string details = null) =>
            new Error { Code = 6, Text = "duplicate_key", Details = details };
    }
}
