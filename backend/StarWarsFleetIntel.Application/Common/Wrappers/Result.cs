using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Application.Common.Wrappers
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }

        public static Result<T> Success(T data, string message = "")
        {
            return new Result<T>
            {
                Succeeded = true,
                Data = data,
                Message = message
            };
        }

        public static Result<T> Failure(string message, List<string>? errors = null)
        {
            return new Result<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
