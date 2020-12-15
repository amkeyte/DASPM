using DASPM_PCTEL.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTEL.Table
{
    /// <summary>
    /// an object that processes some calculation
    /// </summary>
    public interface IDoesCalculation
    {
        void Calculate();
    }

    /// <summary>
    /// Any class that has the DataSetType property
    /// </summary>
    public interface IHasDataSetVariant
    {
        PCTEL_DataSetVariant DataSetVariant { get; }
    }

    /// <summary>
    /// A object that has Location property
    /// </summary>
    public interface IHasLocation
    {
        PCTEL_Location Location { get; }
    }

    /// <summary>
    /// An object that contains a list of locations
    /// </summary>
    public interface IHasLocations
    {
        IList<IHasLocation> Locations { get; }

        void AddLocation(PCTEL_Location loc);

        void RefreshLocations(); //may move this...

        void RemoveLocation(PCTEL_Location loc);
    }
}