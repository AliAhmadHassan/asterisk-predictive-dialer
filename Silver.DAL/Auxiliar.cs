using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Silver.DAL
{
    public class Auxiliar
    {
        public static List<MySqlParameter> GeraParametros<T>(T DadosDTO)
        {
            var LParams = new List<MySqlParameter>();

            foreach (PropertyInfo pi in DadosDTO.GetType().GetProperties())
            {
                MySqlParameter Param;

                if (pi.PropertyType.FullName == "System.DateTime")
                {
                    if (((DateTime)pi.GetValue(DadosDTO, null)).Year == 1)
                    {
                        Param = new MySqlParameter("in" + pi.Name, DateTime.MinValue);
                    }
                    else
                    {
                        Param = new MySqlParameter("in" + pi.Name, pi.GetValue(DadosDTO, null));
                    }
                }
                else
                {
                    if (pi.PropertyType.FullName == "System.Byte[]")
                    {
                        if (pi.GetValue(DadosDTO, null) == null)
                        {
                            Param = new MySqlParameter("in" + pi.Name, DBNull.Value);
                        }
                        else
                        {
                            Param = new MySqlParameter("in" + pi.Name, pi.GetValue(DadosDTO, null));
                        }
                    }
                    else
                    {
                        Param = new MySqlParameter("in" + pi.Name, pi.GetValue(DadosDTO, null));
                    }
                }
                LParams.Add(Param);
            }
            return LParams;
        }
        public static T RetornaDadosEntidade<T>(MySqlDataReader Dr)
        {
            var _entidade = (T)Activator.CreateInstance(typeof(T));

            var LColunas = new List<string>();

            for (var i = 0; i < Dr.FieldCount; i++)
            {
                LColunas.Add(Dr.GetName(i).ToLower());
            }
            foreach (PropertyInfo pi in _entidade.GetType().GetProperties())
            {
                if (!LColunas.Contains(pi.Name.ToLower()))
                {
                    continue;
                }
                else
                {
                    if (Dr[pi.Name] == DBNull.Value)
                    {
                        pi.SetValue(_entidade, null, null);
                    }
                    else
                    {
                        pi.SetValue(_entidade, Dr[pi.Name], null);
                    }
                }
            }
            return _entidade;
        }

        public static Atributos RetornoAtributos<T>(T Entidade)
        {
            Atributos atributo = null;

            var tipo = typeof(T);
            var props = typeof(T).GetProperties();
            ;

            foreach (var p in props)
            {
                var info = tipo.GetProperty(p.Name);

                var Obj = tipo.GetProperty(p.Name).GetCustomAttributes(true);

                if (Obj.Length > 0)
                {
                    if (((DTO.Base.AtributoBind)Obj.GetValue(0)).ChavePrimaria)
                    {
                        atributo = new Atributos();
                        atributo.NomeChavePrimeria = p.Name;
                        atributo.NomeProcedureAlterar = ((DTO.Base.AtributoBind)Obj.GetValue(0)).ProcedureAlterar;
                        atributo.NomeProcedureInserir = ((DTO.Base.AtributoBind)Obj.GetValue(0)).ProcedureInserir;
                        atributo.NomeProcedureRemover = ((DTO.Base.AtributoBind)Obj.GetValue(0)).ProcedureRemover;
                        atributo.NomeProcedureListarTodos = ((DTO.Base.AtributoBind)Obj.GetValue(0)).ProcedureListarTodos;
                        atributo.NomeProcedurePelaPK = ((DTO.Base.AtributoBind)Obj.GetValue(0)).ProcedureSelecionar;
                    }
                }
            }

            if (atributo == null)
            {
                throw new Exception("Campo Chave Não Encontrado");
            }
            return atributo;
        }

        public class Atributos
        {
            public string NomeChavePrimeria { get; set; }

            public string NomeProcedureInserir { get; set; }

            public string NomeProcedureAlterar { get; set; }

            public string NomeProcedureRemover { get; set; }

            public string NomeProcedureListarTodos { get; set; }

            public string NomeProcedurePelaPK { get; set; }
        }
    }
}

