using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using DASPM_PCTEL.DataSet;

namespace DASPM_PCTEL.Updater
{
    public class PCTEL_UpdateWriter<TModel>
        where TModel : PCTEL_UpdaterTableRowModel
    {
        #region ctor

        public PCTEL_UpdateWriter(PCTEL_UpdaterTable<TModel> table)
        {
            Table = table;
        }

        #endregion ctor

        #region ClassMembers

        private List<PCTEL_DataSet<PCTEL_DataSetRowModel>> _dataSets = new List<PCTEL_DataSet<PCTEL_DataSetRowModel>>();

        public string DataSetPath { get => Table.DataSetPath; }

        public List<PCTEL_DataSet<PCTEL_DataSetRowModel>> DataSets
        {
            get
            {
                if (_dataSets.Count == 0)
                {
                    var files = Directory.GetFiles(DataSetPath);
                    foreach (string f in files)
                    {
                        string fName = f.Substring(DataSetPath.Length + 1);
                        _dataSets.Add(PCTEL_DataSet.Create(fName, DataSetPath, fName));
                        _dataSets[_dataSets.Count - 1].LoadFromFile();
                    }
                }
                return _dataSets;
            }
        }

        public PCTEL_UpdaterTable<TModel> Table { get; protected set; }
        public PCTEL_UpdaterRules<TModel> UpdaterRules { get; set; }

        public void Update(string writeToPath = "")
        {
            if (writeToPath == "") writeToPath = DataSetPath;

            foreach (var dataSet in DataSets)
            {
                foreach (PCTEL_DataSetRow<PCTEL_DataSetRowModel> dataSetRow in dataSet.Rows)
                {
                    if (DataSetLocationExists(dataSetRow)) continue;

                    foreach (var dataSetPropInfo in typeof(PCTEL_DataSetRowModel).GetProperties())
                    {
                        if (ContainsIgnoreAttribute(dataSetPropInfo)) continue;

                        var rowModelPropInfo = typeof(TModel).GetProperty(dataSetPropInfo.Name);
                        if (rowModelPropInfo is null) continue;

                        var dataSetFieldVal = dataSetPropInfo.GetValue(dataSetRow.Fields);
                        var rowModelFieldVal = rowModelPropInfo.GetValue(Table[dataSetRow.Location]);

                        UpdatePerRules(dataSetRow, dataSetPropInfo, dataSetFieldVal, rowModelFieldVal);
                    }
                }
                dataSet.WriteToFile(writeToPath, dataSet.Filename);
            }
        }

        private bool ContainsIgnoreAttribute(PropertyInfo pInfo)
        {
            foreach (var attr in pInfo.GetCustomAttributes(true))
            {
                if (attr.ToString().Contains("IgnoreAttribute")) return true;
            }
            return false;
        }

        private bool DataSetLocationExists(PCTEL_DataSetRow<PCTEL_DataSetRowModel> dataSetRow)
        {
            return !Table.Locations.Keys.Contains(dataSetRow.Location);
        }

        private void UpdatePerRules(PCTEL_DataSetRow<PCTEL_DataSetRowModel> dataSetRow,
            PropertyInfo dataSetPropInfo,
            object dataSetFieldVal,
            object rowModelFieldVal)
        {
            switch (UpdaterRules.GetRule(dataSetPropInfo.Name))
            {
                case PCTEL_UpdaterActions.OVERWRITE:
                    dataSetPropInfo.SetValue(dataSetRow.Fields, rowModelFieldVal);
                    break;

                case PCTEL_UpdaterActions.UPDATE_IF_EMPTY:
                    if (dataSetFieldVal is null
                        || (string)dataSetFieldVal == ""
                        && (string)rowModelFieldVal != "")
                    {
                        dataSetPropInfo.SetValue(dataSetRow.Fields, rowModelFieldVal, null);
                    }
                    break;

                case PCTEL_UpdaterActions.DO_NOTHING:
                    //do nothing
                    break;
            }
        }

        #endregion ClassMembers
    }
}