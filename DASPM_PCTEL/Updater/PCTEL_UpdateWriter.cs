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

        public PCTEL_UpdateWriter(PCTEL_UpdaterTable<TModel> table, string dataSetPath)
        {
            Table = table;
            DataSetPath = dataSetPath;
        }

        #endregion ctor

        #region ClassMembers

        private List<PCTEL_DataSet<PCTEL_DataSetRowModel>> _dataSets = new List<PCTEL_DataSet<PCTEL_DataSetRowModel>>();

        public string DataSetPath { get; protected set; }

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

        public void Update(string writeToPath = "")
        {
            if (writeToPath == "") writeToPath = DataSetPath;

            foreach (var dataSet in DataSets)
            {
                foreach (PCTEL_DataSetRow<PCTEL_DataSetRowModel> dataSetRow in dataSet.Rows)
                {
                    if (Table.Locations.Keys.Contains(dataSetRow.Location))
                    {
                        dataSetRow.Fields.Comment = Table[dataSetRow.Location].Comment;
                    }
                }
                dataSet.WriteToFile(writeToPath, dataSet.Filename);
            }
        }

        #endregion ClassMembers
    }
}