using System;
using System.Linq;
using System.Collections.Generic;

namespace Silver.BLL
{
    public class UsuarioLogin
    {
        public void RegistrarCheckin(long codigoUsuario)
        {
            var usuarioLogin = Obter(codigoUsuario).LastOrDefault();
            usuarioLogin.Checkin = DateTime.Now;
            Atualizar(usuarioLogin);
        }

        public void RegistrarSaida(long codigoUsuario)
        {
            var usuarioLogin = Obter(codigoUsuario).LastOrDefault();
            usuarioLogin.Checkin = DateTime.Now;
            usuarioLogin.Logout = DateTime.Now;
            usuarioLogin.TempoSegundos = (long)((usuarioLogin.Logout - usuarioLogin.Login).TotalSeconds);
            Atualizar(usuarioLogin);
        }

        public void RegistrarEntrada(DTO.Usuario usuario)
        {
            var usuarioLogin = Obter(usuario.Id).LastOrDefault();

            if (usuarioLogin != null && usuarioLogin.Logout == DateTime.MinValue)
            {
                usuarioLogin.Logout = usuario.Operador ? usuarioLogin.Checkin : DateTime.Now;
                usuarioLogin.TempoSegundos = (long)((usuarioLogin.Logout - usuarioLogin.Login).TotalSeconds);
                Atualizar(usuarioLogin);
            }

            usuarioLogin = new DTO.UsuarioLogin();
            usuarioLogin.IdUsuario = usuario.Id;
            usuarioLogin.Login = DateTime.Now;
            usuarioLogin.Checkin = DateTime.Now;
            Incluir(usuarioLogin);
        }

        public List<DTO.UsuarioLogin> Listar()
        {
            return new DAL.UsuarioLogin().Select();
        }

        public List<DTO.UsuarioLogin> Obter(long codigoUsuario)
        {
            return new DAL.UsuarioLogin().SelectPeloUsuario(codigoUsuario);
        }

        public DTO.UsuarioLogin Obter(int codigoUsuarioLogin)
        {
            return new DAL.UsuarioLogin().SelectPelaPK(codigoUsuarioLogin);
        }

        public void Incluir(DTO.UsuarioLogin usuarioLogin)
        {
            new DAL.UsuarioLogin().Cadastro(usuarioLogin);
        }

        public void Atualizar(DTO.UsuarioLogin usuarioLogin)
        {
            Incluir(usuarioLogin);
        }
    }
}
