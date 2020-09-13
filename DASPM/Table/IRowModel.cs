﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace DASPM.Table
{
    /// <summary>
    /// The data model for this table. POCO properties contain the data for each field.
    /// </summary>
    public interface IRowModel
    {
        /// <summary>
        /// Convert model to dictionary. Keys are field names, values are field values
        /// </summary>
        /// <returns>The dictionary</returns>
        Dictionary<string, object> ToDict();

        /// <summary>
        /// Convert model to list containing values
        /// </summary>
        /// <returns>the list of values</returns>
        List<object> ToList();

        /// <summary>
        /// A list of headers present in this field
        /// </summary>
        List<string> Headers { get; }
    }
}