using System;
using System.Collections.Generic;
using System.Text;

namespace DDDCommon.Utils
{
    public static class Errors
    {
        public static Error InternalError(object details = null) =>
            new Error(1, "internal_error", details);
        public static Error NotFound(object details = null) =>
            new Error(2, "not_found", details);
        public static Error AlreadyDone(object details = null) =>
            new Error(3, "already_done", details);
        public static Error InvalidOperation(object details = null) =>
            new Error(4, "invalid_operation", details);
        public static Error DatabaseError(object details = null) =>
            new Error(5, "database_error", details);
        public static Error DuplicateKey(object details = null) =>
            new Error(6, "duplicate_key", details);
        public static Error AccessDenied(object details = null) =>
            new Error(7, "access_denied", details);
    }
}
