using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientApplication
{
    internal class ClientException : Exception
    {
        internal string _error;
        public string Error
        {
            get { return _error; }
        }
        internal ClientException(Exception ex)
        {
            _error = ex.Message;
        }
    }
}
