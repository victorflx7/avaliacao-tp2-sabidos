using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Exceptions
{
    internal class AuthorizationException : Exception
    {
        AuthorizationException(string message) : base(message) { }
    }
}
