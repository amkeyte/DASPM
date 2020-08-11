using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASPM_PCTEL.Updater
{
    public enum PCTEL_UpdaterActions
    {
        DO_NOTHING,
        OVERWRITE = 110,
        UPDATE_IF_EMPTY,
    }

    public class PCTEL_UpdaterRules<TModel>
        where TModel : PCTEL_UpdaterTableRowModel
    {
        public Dictionary<string, PCTEL_UpdaterActions> Actions = new Dictionary<string, PCTEL_UpdaterActions>();

        public PCTEL_UpdaterRules()
        {
            Actions.Add("Comment", PCTEL_UpdaterActions.UPDATE_IF_EMPTY);
        }

        public PCTEL_UpdaterActions GetRule(string target)
        {
            return Actions[target];
        }
    }
}