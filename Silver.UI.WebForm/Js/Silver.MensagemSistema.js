
$(document).ready(function () {
    $.jGrowl.defaults.closerTemplate = "<div>[ fechar tudo ]</div>";

    $('.link_marcacao').click(function () {
        ExibirMensagem($(this).text());
    });
});

var intervalo_atualizacao = 1000 * 60;
var intervalo_consulta_mensagem = self.setInterval(function () { VerificarMensagemSistema() }, 10000);

function ExibirMensagem(msg) {
    var res = msg.split('|');
    for (var i = 0; i < res.length; i++) {
        if (res[i].length > 0) {
            $.jGrowl(res[i], {
                closer: false,
                sticky: true,
                glue: 'before',
                speed: 1000,
                header: 'Mensagem Silver - ' + ObterData()
            });
        }
    }
}

function abrir_popup(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=yes, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

function VerificarMensagemSistema() {

    clearInterval(intervalo_consulta_mensagem);
    urlToHandler = 'http://localhost:60522/NotificacaoSistema.ashx';
    jsonData = '';

    $.ajax({
        url: urlToHandler,
        data: jsonData,
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            try {
                if (data.responseUrl == "") {
                    ExibirMensagem(data.responseMsg);
                } else {
                    ExibirMensagem(data.responseUrl);
                }
            } catch (e) {
                ExibirMensagem(e + " " + data.responseMsg);
            }
            finally {
                intervalo_consulta_mensagem = self.setInterval(function () { VerificarMensagemSistema() }, intervalo_atualizacao);
            }
        },
        error: function (data, status, jqXHR) {
            $("#tabs").tabs({ active: 2 });
            ExibirMensagem('Falha na conexão com o servidor remoto. \'Se esta mensagem continuar\', informe seu supervisor sobre o problema. Status: ' + status + ', Variável: ' + jqXHR);
            intervalo_consulta_mensagem = self.setInterval(function () { VerificarMensagemSistema() }, intervalo_atualizacao);
        }
    });

};

function ObterData() {
    // Obtém a data/hora atual
    var data = new Date();

    var dia = data.getDate();
    var dia_sem = data.getDay();
    var mes = data.getMonth();
    var ano2 = data.getYear();
    var ano4 = data.getFullYear();
    var hora = data.getHours();
    var min = data.getMinutes();
    var seg = data.getSeconds();
    var mseg = data.getMilliseconds();
    var tz = data.getTimezoneOffset();

    var str_data = dia + '/' + (mes + 1) + '/' + ano4;
    var str_hora = hora + ':' + min + ':' + seg;

    return str_data + ' ' + str_hora;
}