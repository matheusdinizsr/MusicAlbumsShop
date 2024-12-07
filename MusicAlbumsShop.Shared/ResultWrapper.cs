using MusicAlbumsShop.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAlbumsShop.Shared
{   
    public class ResultWrapper <T>
    {
        public T Value { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public void SetSuccess(T value)
        {
            Value = value;
            IsSuccess = true;
            ErrorMessage = string.Empty;

        }

        public void SetError(string message)
        {
            Value = default;
            IsSuccess = false;
            ErrorMessage = message;
        }
    }
}
