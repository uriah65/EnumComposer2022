using EnumComposer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    class Fake33_
    {
        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source =..\..\T33\AccessTest.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name FROM T_Colors")]
        public enum ColorsEnum
        {
			//Red = 1,
			//Blue = 2,
			//Green = 3,
		}

        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source =..\..\T33\AccessTest.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name, description FROM T_Colors")]
        public enum ColorsWithDescriptionEnum
        {
			//[Description("The color of tomato")]
			//Red = 1,
			[Description("Blue is the colour of the clear sky and the deep sea.[2][3] It is located between \" violet and \" ] [ green [on the(\"\"\") optical] spect {} ru )( - m.")]
			Blue = 2,
			[Description("Grass is green")]
			Green = 3,
		}
    }
}
