$(function () {
    $("#accordion_carga").accordion();

    $(document).tooltip({ position: { my: "left center", at: "right+10 center", collision: "flipfit" }, tooltipClass: "tooltip_classe" });

    $("#tabs_grid").tabs({ collapsible: true });

    $("#tabs_ligacoes").tabs({ collapsible: true });

    $("#tabs_mailing").tabs({ collapsible: true });

    $("#txt_cpfcnpj").focusin(function () {
        $("#txt_ramaloperador").val('');
    });

    $("#txt_ramaloperador").focusin(function () {
        $("#txt_cpfcnpj").val('');
    });
});

function Logout(control) {
    if (confirm('Deseja sair do sistema?')) {
        __doPostBack(control, '');
    }
}

function LimparFormulario(ele) {
    $(ele).find(':input').each(function () {
        switch (this.type) {
            case 'password':
            case 'select-multiple':
            case 'select-one':
            case 'text':
            case 'textarea':
                $(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
    });
}

function abrir_popup(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

function SetarValorAndamento(id, valor) {
    if (id == 1) {
        $('#div_progresso_1').progressbar({ value: valor });
    } else if (id == 2) {
        $('#div_progresso_2').progressbar({ value: valor });
    } else if (id == 2) {
        $('#div_progresso_3').progressbar({ value: valor });
    } else if (id == 2) {
        $('#div_progresso_4').progressbar({ value: valor });
    } else {
        $('#div_progresso_5').progressbar({ value: valor });
    }
};


