using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Petshop.Utils
{
    public static class Utils
    {
        #region Acessibilidade
        public static void FonteMais(Window w, Menu m)
        {
            FonteMaisW(w);
            FonteMaisM(m);
        }
        public static void FonteMenos(Window w, Menu m)
        {
            FonteMenosW(w);
            FonteMenosM(m);
        }

        public static void FonteMaisW(Window w)
        {
            if(w.FontSize < 30)
                w.FontSize++;
        }
        public static void FonteMenosW(Window w)
        {
            if(w.FontSize > 9)
                w.FontSize--;
        }

        public static void FonteMaisM(Menu m)
        {
            if (m.FontSize < 30)
                m.FontSize++;
        }
        public static void FonteMenosM(Menu m)
        {
            if (m.FontSize > 9)
                m.FontSize--;
        }
        #endregion
    }
}
