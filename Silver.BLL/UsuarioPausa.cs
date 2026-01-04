using System;
using System.Collections.Generic;
using System.Linq;

namespace Silver.BLL
{
    public class UsuarioPausa
    {
        public bool IniciarPausa(DTO.Usuario usuario, long codigoPausa)
        {
            try
            {
                var usuarioPausa = Obter(usuario.Id).LastOrDefault();

                if (usuarioPausa != null && usuarioPausa.Fim == DateTime.MinValue)
                {
                    usuarioPausa.Fim = new BLL.UsuarioLogin().Obter(usuario.Id).LastOrDefault().Checkin;
                    usuarioPausa.TempoSegundos = (long)((usuarioPausa.Fim - usuarioPausa.Inicio).TotalSeconds);
                    Atualizar(usuarioPausa);
                }

                usuarioPausa = new DTO.UsuarioPausa();
                usuarioPausa.IdPausa = codigoPausa;
                usuarioPausa.IdUsuario = usuario.Id;
                usuarioPausa.Inicio = DateTime.Now;
                Incluir(usuarioPausa);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RetornarPausa(long codigoUsuario)
        {
            try
            {
                var usuarioPausa = Obter(codigoUsuario).LastOrDefault();
                usuarioPausa.Fim = DateTime.Now;
                usuarioPausa.TempoSegundos = (long)((usuarioPausa.Fim - usuarioPausa.Inicio).TotalSeconds);
                Atualizar(usuarioPausa);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<DTO.UsuarioPausa> Listar()
        {
            return new DAL.UsuarioPausa().Select();
        }

        public List<DTO.UsuarioPausa> Obter(long codigoUsuario)
        {
            return new DAL.UsuarioPausa().SelectPeloUsuario(codigoUsuario);
        }

        /// <summary>
        /// Retorna as pausas que estão entre os periodos informados
        /// </summary>
        /// <param name="dt_inicio"></param>
        /// <param name="dt_fim"></param>
        /// <returns></returns>
        public List<DTO.UsuarioPausa> Obter(DateTime dt_inicio, DateTime dt_fim)
        {
            return new DAL.UsuarioPausa().SelectPeloInicio(dt_inicio, dt_fim);
        }

        public DTO.UsuarioPausa Obter(int codigoUsuarioPausa)
        {
            return new DAL.UsuarioPausa().SelectPelaPK(codigoUsuarioPausa);
        }

        public void Incluir(DTO.UsuarioPausa usuarioPausa)
        {
            new DAL.UsuarioPausa().Cadastro(usuarioPausa);
        }

        public void Atualizar(DTO.UsuarioPausa usuarioPausa)
        {
            Incluir(usuarioPausa);
        }
    }
}
