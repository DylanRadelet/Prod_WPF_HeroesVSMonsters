using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Heroes_VS_Monsters.Class.Monstre
{
    public class Loup : Monstre
    {
        #region Properties

        public override double BonusEnd
        {
            get => 1;
        }

        public override int Cuir
        {
            get => 5;
        }

        public override int Gold
        {
            get => 6;
        }

        #endregion


        #region constructor
        public Loup(string race, int @for, int end, double pdv, double position, int gold = 6, int cuir = 5)
            : base(@for, end, pdv, position, gold, cuir, race)
        {
        }

        #endregion
    }
}
