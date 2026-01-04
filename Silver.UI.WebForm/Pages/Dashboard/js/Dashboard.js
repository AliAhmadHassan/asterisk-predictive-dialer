$(document).ready(function () {
   
    $('#btn_opcoes')
              .hover(function () {
                  $("#div_opcoes").show("fast", function () { height: 500; });
              }, function () {
                  $("#div_grupos").hide("fast", function () { });
                  $("#div_carga").hide("fast", function () { });
                  return;
              });

    $('#div_opcoes')
              .hover(function () {
                  return;
              }, function () {
                  $("#div_opcoes").slideUp("fast", function () { });
              });

    $('#btn_grupos')
              .hover(function () {
                  $("#div_grupos").show("fast", function () {
                  });
              }, function () {
                  $("#div_opcoes").hide("fast", function () { });
                  $("#div_carga").hide("fast", function () { });
                  return;
              });

    $('#div_grupos')
              .hover(function () {
                  return;
              }, function () {
                  $("#div_grupos").slideUp("fast", function () { });
              });

    $('#btn_carga')
              .hover(function () {
                  $("#div_carga").show("fast", function () {

                  });
              }, function () {
                  $("#div_opcoes").hide("fast", function () { });
                  $("#div_grupos").hide("fast", function () { });
                  return;
              });

    $('#div_carga')
              .hover(function () {
                  return;
              }, function () {
                  $("#div_carga").slideUp("fast", function () { });
              });

});

function secondsToHms(d) {
    d = Number(d);
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);
    if (h < 9) { h = "0" + h; }
    if (m < 9) { m = "0" + m; }
    if (s < 9) { s = "0" + s; }
    return (h + ":" + m + ":" + s);
}