﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DASPM.Table;
using DASPM_PCTEL.Table;

namespace DASPM_PCTEL.DataSet
{
    internal class PCTEL_DataSetRowCore
    {
        protected PCTEL_DataSetRow TableRow { get; set; }

        public PCTEL_DataSetRowCore(PCTEL_DataSetRow tableRow)
        {
            TableRow = tableRow;
        }
    }

    public class PCTEL_DataSetRow : PCTEL_TableRow
    {
        #region ctor

        public static PCTEL_DataSetRow Create(PCTEL_DataSet table, PCTEL_DataSetRowModel model)
        {
            return (PCTEL_DataSetRow)Create(table, model, typeof(PCTEL_DataSetRow));
        }

        public static implicit operator PCTEL_DataSetRow(PCTEL_DataSetRow<PCTEL_DataSetRowModel> other)
        {
            //double cast allows cross-generic conversion??
            return (PCTEL_DataSetRow)(ITableRow)other;
        }

#pragma warning disable IDE0052 // Remove unread private members

        //future use
        private PCTEL_DataSetRowCore Core { get; set; }

#pragma warning restore IDE0052 // Remove unread private members

        public PCTEL_DataSetRow()
        {
            Core = new PCTEL_DataSetRowCore(this);
        }

        #endregion ctor

        #region ClassMembers

        //neeed this?
        //public new PCTEL_DataSet<T> Table { get; protected set; }
        //Convenience casting
        public new PCTEL_DataSetRowModel Fields
        {
            get
            {
                return (PCTEL_DataSetRowModel)base.Fields;
            }
        }

        #endregion ClassMembers
    }

    public class PCTEL_DataSetRow<TModel> : PCTEL_TableRow<TModel>
        where TModel : PCTEL_DataSetRowModel
    {
        #region ctor

        public static PCTEL_DataSetRow<TModel> CreateGeneric(PCTEL_DataSet<TModel> table, TModel model)
        {
            return (PCTEL_DataSetRow<TModel>)CreateGeneric(table, model, typeof(PCTEL_DataSetRow<TModel>));
        }

        //not sure if needed...

        //public static implicit operator PCTEL_DataSetRow<TModel>(PCTEL_DataSetRow other)
        //{
        //    //double cast allows cross-generic conversion??
        //    return (PCTEL_DataSetRow<TModel>)(ITableRow)other;
        //}

#pragma warning disable IDE0052 // Remove unread private members

        //future use
        private PCTEL_DataSetRowCore Core { get; set; }

#pragma warning restore IDE0052 // Remove unread private members

        public PCTEL_DataSetRow()
        {
            Core = new PCTEL_DataSetRowCore((PCTEL_DataSetRow)(ITableRow)this);
        }

        #endregion ctor

        #region ClassMembers

        //neeed this?
        //public new PCTEL_DataSet<T> Table { get; protected set; }
        //Convenience casting
        public new PCTEL_DataSetRowModel Fields
        {
            get
            {
                return (PCTEL_DataSetRowModel)base.Fields;
            }
        }

        #endregion ClassMembers
    }
}