using System;
using System.Collections.Generic;
using System.Linq;

namespace Silver.UI.Web.Presentation.Requests
{
    public static class UsuarioOnLine
    {
        public static SortedList<long, DateTime> usuarioOnLine_checking = new SortedList<long, DateTime>();
        public static SortedList<long, string> usuarioOnLine = new SortedList<long, string>();
        public static SortedList<long, string> ProximoCliente = new SortedList<long, string>();
    }
}
