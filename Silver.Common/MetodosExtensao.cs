using System;
using System.Web;

namespace Silver.Common
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    /// Data: 25/04/2013
    /// </summary>
    public static class MetodosExtensao
    {
        public static Int16 ToInt16(this string value)
        {
            Int16 _valor = 0;
            if (Int16.TryParse(value, out _valor))
            {
                return _valor;
            }
            return 0;
        }

        public static Int32 ToInt32(this string value)
        {
            Int32 _valor = 0;
            if (Int32.TryParse(value, out _valor))
            {
                return _valor;
            }
            return 0;
        }

        public static Int64 ToInt64(this string value)
        {
            Int64 _valor = 0;
            if (Int64.TryParse(value, out _valor))
            {
                return _valor;
            }
            return 0;
        }

        public static Int16 ToInt16(this object value)
        {
            Int16 valor = 0;
            if (!Int16.TryParse(value.ToString(), out valor))
            {
                throw new ArgumentException("Parâmentro inválido");
            }
            return valor;
        }

        public static Int32 ToInt32(this object value)
        {
            try
            {
                Int32 valor = 0;
                if (!Int32.TryParse(value.ToString(), out valor))
                {
                    throw new ArgumentException("Parâmentro inválido");
                }
                return valor;
            }
            catch
            {
                return 0;
            }
        }

        public static Int64 ToInt64(this object value)
        {
            Int64 valor = 0;
            if (!Int64.TryParse(value.ToString(), out valor))
            {
                throw new ArgumentException("Parâmentro inválido");
            }
            return valor;
        }

        public static Int32 ToInt32(this bool value)
        {
            var result = false;
            if (!bool.TryParse(value.ToString(), out result))
            {
                throw new ArgumentException("O parâmetro informado não é um tipo booleano válido");
            }
            return Convert.ToInt32(result);
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime date = new DateTime();
            DateTime.TryParse(value, out date);
            return date;
        }

        public static Decimal ToDecimal(this string value)
        {
            Decimal _valor = 0;
            if (Decimal.TryParse(value, out _valor))
            {
                return _valor;
            }
            return 0;
        }

        public static Uri Combine(this Uri relativeBaseUri, Uri relativeUri)
        {
            if (relativeBaseUri == null)
            {
                throw new ArgumentNullException("relativeBaseUri");
            }

            if (relativeUri == null)
            {
                throw new ArgumentNullException("relativeUri");
            }

            string baseUrl = VirtualPathUtility.AppendTrailingSlash(relativeBaseUri.ToString());
            string combinedUrl = VirtualPathUtility.Combine(baseUrl, relativeUri.ToString());

            return new Uri(combinedUrl, UriKind.Relative);
        }
    }
}
