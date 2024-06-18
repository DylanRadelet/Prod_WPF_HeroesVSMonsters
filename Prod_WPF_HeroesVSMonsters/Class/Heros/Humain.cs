using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes_VS_Monsters.Class.Heros
{
    public class Humain : Heros
    {

        #region Properties
        public override double BonusFor => 1;
        public override double BonusEnd => 1;

        #endregion



        #region constructor
        public Humain(string name, int @for, int end, double pdv, double position, int gold, int cuir)
            : base(name, @for, end, pdv, position, gold, cuir)
        {
        }

        #endregion

    }
}
