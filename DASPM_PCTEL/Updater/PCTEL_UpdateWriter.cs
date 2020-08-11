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
                    if (Table.Locations.Keys.Contains(dataSetRow.Location))
                    {
                        var t1 = typeof(PCTEL_DataSetRowModel);
                        var t2 = typeof(PCTEL_UpdaterTableRowModel);
                        var pi1 = t1.GetProperties();

                        foreach (var pi1Iter in pi1)
                        {
                            //var pi1IterAttr = pi1Iter.GetCustomAttributes(true);
                            if (ContainsIgnoreAttribute(pi1Iter)) continue;

                            var oldField = pi1Iter.GetValue(dataSetRow.Fields);
                            var p2 = t2.GetProperty(pi1Iter.Name);
                            if (p2 is null) continue;
                            var newField = p2.GetValue(Table[dataSetRow.Location]);

                            switch (UpdaterRules.GetRule(pi1Iter.Name))
                            {
                                case PCTEL_UpdaterActions.OVERWRITE:
                                    pi1Iter.SetValue(oldField, newField);
                                    break;

                                case PCTEL_UpdaterActions.UPDATE_IF_EMPTY:
                                    if (oldField is null || (string)oldField == "" && (string)newField != "")
                                    {
                                        pi1Iter.SetValue(oldField, newField);
                                    }
                                    break;

                                case PCTEL_UpdaterActions.DO_NOTHING:
                                    //do nothing
                                    break;
                            }
                        }
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

        #endregion ClassMembers
    }
}