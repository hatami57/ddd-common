using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDDCommon.Utils
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Error Error { get; set; }

        public OperationResult(bool success, Error error = null)
        {
            Success = success;
            Error = error;
        }

        public static async Task<OperationResult> Try(Func<Task> func)
        {
            try
            {
                await func();
                return new OperationResult(true);
            }
            catch (Exception e)
            {
                return new OperationResult(false,
                    e as Error ?? Errors.InternalError(e.Message));
            }
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public OperationResult(bool success, T result, Error error = null)
            : base(success, error)
        {
            Result = result;
        }

        public static async Task<OperationResult<T>> Try(Func<Task<T>> func)
        {
            try
            {
                return new OperationResult<T>(true, await func(), null);
            }
            catch (Exception e)
            {
                return new OperationResult<T>(false, default,
                    e as Error ?? Errors.InternalError(e.Message));
            }
        }
    }

    [JsonConverter(typeof(ErrorJsonConverter))]
    public class Error : Exception
    {
        public int Code { get; set; }
        public string Text { get; set; }
        public string Details { get; set; }
    }
}
