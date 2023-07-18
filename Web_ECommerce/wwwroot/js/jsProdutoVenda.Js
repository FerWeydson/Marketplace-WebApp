var objetoVenda = new Object();

objetoVenda.AdicionarCarrinho = function (idProduto) {

    let nome = $("#nome_" + idProduto).val();
    let qtd = $("#qtd_" + idProduto).val();

    $.ajax({
        type: 'POST',
        url: '/api/AdicionarProdutoCarrinho',
        dataType: 'JSON',
        cache: false,
        async: true,
        data: {
            "id": idProduto,  "nome": nome,  "qtd": qtd 
        },
        success: function (data) {
            if (data.sucesso) {

                ObjetoAlerta.AlertaTela(1, "Produto adicionado no carrinho");

                } else{

                ObjetoAlerta.AlertaTela(2, "Usuário não logado");
            }
        }

    });

};


objetoVenda.CarregaProdutos = function ()
{
    $.ajax({
        type: 'GET',
        url: '/api/ListarProdutoComEstoque',
        dataType: 'JSON',
        cache: false,
        async: true,
        success: function (data) {

            let htmlConteudo = "";

            data.forEach(function (Entitie) {
                htmlConteudo += " <div class='col-xs-12 col-sm-4 col-md-4 col-lg-4'>"

                let idNome = "nome_" + Entitie.id;
                let idQtd = "qtd_" + Entitie.id;

                htmlConteudo += "<label id='" + idNome + "' > Produto: " + Entitie.nome + "</label></br>";
                if (Entitie.url != null && Entitie.url != "" && Entitie.url != undefined) {
                    htmlConteudo += "<img width='200' height='100' src='" + Entitie.url + "'></br>";
                }
                htmlConteudo += "<label> Valor: " + Entitie.valor + "</label ></br>";

                htmlConteudo += "<label> Quantidade : <input type'number' value='1' id='" + idQtd + "'</label></br>";
                htmlConteudo += "<input type='button' onclick='objetoVenda.AdicionarCarrinho(" + Entitie.id + ")' value= 'Comprar' </br>";

                htmlConteudo += " </div>";
            });
            $("#DivVenda").html(htmlConteudo);
        }
    })
};

objetoVenda.CarregarQtdCarrinho = function () {

    $("#qtdCarrinho").text("(0)");
    $.ajax({
        type: 'GET',
        url: '/api/QtdProdutoCarrinho',
        dataType: 'JSON',
        cache: false,
        async: true,
        success: function (data) {
            if (data.sucesso) {
                $("#qtdCarrinho").text("(" + data.qtd + ")");
            }
        }
    });
       
    setTimeout(objetoVenda.CarregarQtdCarrinho, 10000);
}

$(function () {
    objetoVenda.CarregaProdutos();
    objetoVenda.CarregarQtdCarrinho();
});











