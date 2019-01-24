/* Filename: Program.cs 
 * Author: Marcin Ostrowski
 * Email: <ostrowski.marcin.gno@gmail.com>
 * Description: This program create package deployment for L3DS Application updater.
 * License: LGPL v3
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L3DS
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new L3DS());
        }
    }
}
