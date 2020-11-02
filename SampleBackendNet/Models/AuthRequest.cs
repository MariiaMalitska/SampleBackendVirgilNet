﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleBackendNet.Models
{
    public class AuthRequest
    {
        [Required]
        public string Identity { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
