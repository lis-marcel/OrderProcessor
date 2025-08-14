using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class EditUserDto
    {
        public string? CurrentName { get; set; }
        public string? NewName { get; set; }
        public string? CurrentEmail { get; set; }
        public string? NewEmail { get; set; }
    }
}
