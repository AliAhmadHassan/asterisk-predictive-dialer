var contador = 1;
var url_encontrada = "";
var cpf_encotrado = "";
var lbl_proximotelefone = "";
var lbl_proximonome = ""

var intervalo_consulta = self.setInterval(function () { VerificarCliente() }, 1000);

function btIniciarPausa_Click(arg) {
    clearInterval(intervalo_consulta);
    var pausa_selecionada = $('#lstPausas').find(":selected").text();
    AddCpf('Pausa Iniciada: ' + pausa_selecionada);
    __doPostBack('btIniciarPausa', arg)
}

function btRetornarPausa_Click(arg) {
    intervalo_consulta = self.setInterval(function () { VerificarCliente() }, 1000);
    var pausa_selecionada = $('#lstPausas').find(":selected").text();
    AddCpf('Pausa Finalizada: ' + pausa_selecionada);
    __doPostBack('btRetornarPausa', arg)
}

$(document).ready(function () {

    AddCpf('Login no sistema.');

    $('#lnk_limpar_log').click(function () {
        var ul = document.getElementById("lista_log");
        LimparListaLog(ul);
    });

    //Configuração dos tooltips
    $(document).tooltip({ track: true, opacity: 0.95 });

    $('#lnk_atender').click(function () {
        if (url_encontrada == "")
            AddLog("Nenhuma url foi encontrada!");
        else
            abrir_popup(url_encontrada, 'CobNet', 1024, 768);
    });

    $(function () {
        $("#accordion_logs").accordion({ collapsible: true });
    });
});


function VerificarCliente() {

    //Limpar Timer para evitar concorrencia na consulta
    clearInterval(intervalo_consulta);

    urlToHandler = 'Helper.ashx';

    var ramal = document.getElementById('lbRamal').innerText;

    jsonData = '{ "ramal":"' + ramal + '" }';
    $.ajax({
        url: urlToHandler,
        data: jsonData,
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            try {
                if (data.responseUrl == "") {
                    AddLog(data.responseMsg);
                } else {
                    url_encontrada = data.responseUrl;
                    
                    document.getElementById('lbl_proximo_cpf').innerText = getQueryStringValue('CPFCNPJ');
                    document.getElementById('lbl_proximo_telefone').innerText = getQueryStringValue('Telefone');
                    document.getElementById('lbl_proximo_nome').innerText = data.responseCliente;

                    AddCpf('CPF/CNPJ: ' + getQueryStringValue('CPFCNPJ') + ', CLIENTE: ' + data.responseCliente + ', TELEFONE: ' + getQueryStringValue('Telefone'));
                    abrir_popup(data.responseUrl, 'Cobnet', 1024, 768);
                }
            } catch (e) {
                alert(e + " " + data.responseMsg);
            }
            finally {
                intervalo_consulta = self.setInterval(function () { VerificarCliente() }, 1000);
            }
        },
        error: function (data, status, jqXHR) {
            $("#tabs").tabs({ active: 2 });
            AddLog('Falha na conexão com o servidor remoto. \'Se esta mensagem continuar\', informe seu supervisor sobre o problema. Status: ' + status + ', Variável: ' + jqXHR);
            intervalo_consulta = self.setInterval(function () { VerificarCliente() }, 1000);
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
    var str_hora = hora + ':' + min + ':' + seg + "." + mseg;

    return str_data + ' ' + str_hora;
}

function LimparListaLog(lista_log) {
    while (lista_log.firstChild) {
        lista_log.removeChild(lista_log.firstChild);
    }
}

function AddCpf(msg) {

    var ul = document.getElementById("lista_cpf_atendidos");
    var newLI = document.createElement("LI");

    newLI.innerHTML = '[ ' + ObterData() + ' ] - ' + msg;
    ul.appendChild(newLI);
}

function AddLog(msg) {
    var ul = document.getElementById("lista_log");
   

    if (contador > 20) {
        contador = 1;
        LimparListaLog(ul);
    }

    var newLI = document.createElement("LI");

    if (msg.indexOf("Sua sessão com o servidor foi perdida") > -1) {
        $("#tabs").tabs({ active: 2 });
    }

    ul.appendChild(newLI);
    newLI.innerHTML = '[' + ObterData() + '] - ' + msg;
    contador++;
}

function abrir_popup(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=yes, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

/*Retorna o valor da querystring passada no parametro*/
function getQueryStringValue(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(url_encontrada);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

$(function () {
    var icons = {
        header: "ui-icon-circle-arrow-e",
        activeHeader: "ui-icon-circle-arrow-s"
    };
    $("#accordion_logs").accordion({
        icons: icons
    });
    $("#toggle").button().click(function () {
        if ($("#accordion_logs").accordion("option", "icons")) {
            $("#accordion_logs").accordion("option", "icons", null);
        } else {
            $("#accordion_logs").accordion("option", "icons", icons);
        }
    });
});

$(function () {
    $("#tabs").tabs();
});