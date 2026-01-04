using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Silver.DTO;

namespace ProjetoTeste
{
    /// <summary>
    /// Classe Responsável pelos testes da DLL DTO
    /// Desenvolvida por: Francisco Silva   
    ///             Data: 09/08/2013
    /// </summary>
    [TestFixture()]
    public class TesteDTO
    {
        [Test(Description = "Construtor das Classes")]
        public void TestarCriacao()
        {
            var a = new AcompanhamentoThread();
            var b = new AmdType();
            var c = new Bilhetagem();
            var d = new Campanha();
            var e = new CampanhaPausa();
            var f = new CampanhaTarefa();
        }
    }
}
