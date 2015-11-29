﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBuilder.Model.Database
{
    [Table(Name = "Person")]
    public class Person
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public Int64 id { get; set; }

        [Column]
        public string Pid { get; set; }

        private EntitySet<Capture> _Captures;
        [Association(Storage = "_Captures", OtherKey = "PersonId")]
        public EntitySet<Capture> Captures
        {
            get { return this._Captures; }
            set { this._Captures.Assign(value); }
        }
    }
}
