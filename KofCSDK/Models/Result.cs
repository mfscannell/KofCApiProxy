using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KofCSDK.Models;

public class Result
{
    public bool Success { get; set; }
    public ProblemDetails Error { get; set; }
    public string RawContent { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class Result<T> : Result
{
    public T Data { get; set; }
}
