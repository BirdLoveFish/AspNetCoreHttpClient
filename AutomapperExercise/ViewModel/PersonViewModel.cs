﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomapperExercise.Model
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentViewModel> Students { get; set; } 
    }
}
