using System;

namespace VacationRental.Core.Exceptions
{
    public sealed class OverbookingException : ApplicationException
    {
        public OverbookingException() : base("Not available")
        {
        }
    }
}
