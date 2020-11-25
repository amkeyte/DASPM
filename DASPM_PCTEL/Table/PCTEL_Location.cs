using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if BUILD_DATASET
using DASPM_PCTEL.DataSet;
#endif

namespace DASPM_PCTEL.Table
{
    public class PCTEL_Location : IComparable
    {
        #region ClassMembers

        public string Floor { get; protected set; }
        public string GridID { get; protected set; }

        public string Key
        {
            get
            {
                return (LocType ?? "")
                    + (Floor ?? "")
                    + (GridID ?? "")
                    + (LocID ?? "");
            }
        }

        public string Label { get; protected set; }
        public string LocID { get; protected set; }
        public string LocType { get; protected set; }

        #endregion ClassMembers

        #region ctor

        public PCTEL_Location(PCTEL_TableRowModel model)
        {
            LocType = model.LocType;
            Floor = model.Floor;
            GridID = model.GridID;
            Label = model.Label;
            LocID = model.LocID;
        }

#if BUILD_DATASET
        //TODO Move this shit out of here to to DataSet namespace.
        public PCTEL_Location(PCTEL_DataSetRowModel model)
        {
            switch (model.DataSetType)
            {
                case PCTEL_DataSetTypes.PCTEL_DST_AREA:
                    LocType = "AREA";
                    break;

                case PCTEL_DataSetTypes.PCTEL_DST_CP:
                    LocType = "CP";
                    break;

                case PCTEL_DataSetTypes.PCTEL_DST_REF:
                    LocType = "REF";
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Bad DataSetType");
            }

            Floor = model.Floor;
            GridID = model.GridID;
            Label = model.Label;
            LocID = model.LocID;
        }
#endif

        #endregion ctor

        #region object

        public static bool operator !=(PCTEL_Location lhs, PCTEL_Location rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(PCTEL_Location lhs, PCTEL_Location rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PCTEL_Location);
        }

        public bool Equals(PCTEL_Location obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (this.GetType() != obj.GetType()) return false;
            return this.Key == obj.Key;
        }

        public override int GetHashCode()
        {
            var hashCode = 374599110;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Floor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GridID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LocID);
            return hashCode;
        }

        #endregion object

        #region IComparable

        public int CompareTo(object obj)
        {
            return CompareTo(obj as PCTEL_Location);
        }

        public int CompareTo(PCTEL_Location loc)
        {
            return Key.CompareTo(loc.Key);
        }

        #endregion IComparable
    }
}