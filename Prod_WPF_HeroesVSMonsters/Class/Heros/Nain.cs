using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes_VS_Monsters.Class.Heros
{
    public class Nain : Heros
    {
        
        #region Properties

        public virtual double BonusEnd
        {
            get => 1;
        }

        #endregion



        #region constructor
        public Nain(string name, int @for, int end, double pdv, double position, int gold, int cuir)
            : base(name, @for, end, pdv, position, gold, cuir)
        {
        }
        #endregion

    }
}
