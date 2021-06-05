using System.Collections.Generic;

namespace Postera.WebApp.Data
{
    public class Result<T>
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; } = true;

        public List<string> Errors { get; set; }
    }
}