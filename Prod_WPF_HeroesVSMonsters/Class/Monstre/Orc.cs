using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes_VS_Monsters.Class.Monstre
{
    public class Orc : Monstre
    {

        #region Properties

        public override double BonusFor
        {
            get => 1;
        }

        public override int Gold
        {
            get => 5;
        }

        #endregion


        #region constructor
        public Orc(string race, int @for, int end, double pdv, double position, int gold = 5, int cuir = 0)
            : base(@for, end, pdv, position, gold, cuir, race)
        {
        }

        #endregion
    }
}
