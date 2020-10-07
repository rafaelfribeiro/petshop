using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Petshop.DAL
{
    class SingletonContext
    {
        private static Context _context;

        public static Context GetInstance()
        {
            if (_context == null)
            {
                _context = new Context();
            }

            return _context;
        }
    }
}
