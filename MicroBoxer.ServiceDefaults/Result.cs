using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBoxer.ServiceDefaults
{
        public readonly struct Result<TValue>
        {
            private readonly TValue? _value;
            private readonly Exception? _error;
            public bool IsError { get; }
            public bool IsSuccess => !IsError;

        public Result(TValue value)
            {
                IsError = false;
                _value = value;
                _error = default;
            }
            public Result(Exception error)
            {
                IsError = true;
                _value = default;
                _error = error;
            }



            public static implicit operator Result<TValue>(TValue value) => new(value);
            public static implicit operator Result<TValue>(Exception error) => new(error);

            public TResult Match<TResult>(
                Func<TValue, TResult> success,
                Func<Exception, TResult> failure) => !IsError ? success(_value!) : failure(_error!);
        }

    
}
