using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        var typex = Type.GetType(nameof(PCTEL_DataSetRowModel), true);
                        foreach (var propInfo in Type.GetType(nameof(PCTEL_DataSetRowModel)).GetProperties())
                        {
                            var oldField = propInfo.GetValue(dataSetRow.Fields);
                            var newField = propInfo.GetValue(Table[dataSetRow.Location]);
                            {
                                switch (UpdaterRules.GetRule(propInfo.Name))
                                {
                                    case PCTEL_UpdaterActions.OVERWRITE:
                                        propInfo.SetValue(oldField, newField);
                                        break;

                                    case PCTEL_UpdaterActions.UPDATE_IF_EMPTY:
                                        if (oldField is null || (string)oldField == "" && (string)newField != "")
                                        {
                                            propInfo.SetValue(oldField, newField);
                                        }
                                        break;

                                    case PCTEL_UpdaterActions.DO_NOTHING:
                                        //do nothing
                                        break;
                                }
                            }
                        }
                    }
                }
                dataSet.WriteToFile(writeToPath, dataSet.Filename);
            }
        }

        #endregion ClassMembers
    }
}