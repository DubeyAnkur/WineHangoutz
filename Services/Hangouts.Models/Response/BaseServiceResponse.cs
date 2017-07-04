using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangout.Models
{
    public abstract class BaseServiceResponse
    {
        public int ErorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
