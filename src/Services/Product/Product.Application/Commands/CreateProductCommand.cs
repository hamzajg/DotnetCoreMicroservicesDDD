using Domain.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Application.Commands
{
    public class CreateProductCommand : Command
    {
        public string Title { get; set; }
        public float OldPrice { get; set; }
        public float CurrentPrice { get; set; }
    }
}
