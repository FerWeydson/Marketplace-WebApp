var objetoVenda = new Object();

objetoVenda.AdicionarCarrinho = function (idProduto) {

    let nome = $("#nome_" + idProduto).val();

    $.ajax({
        type: 'POST',
        url: '/api/AdicionarProdutoCarrinho',
        dataType: 'JSON',
        cache: false,
        async: true,
        data: {
            "id": idProduto,  "nome": nome
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
                htmlConteudo += "<div>"

                let idNome = "nome_" + Entitie.id;


                htmlConteudo += "<label id='" + idNome + "' > " + Entitie.nome + "</label><br/>";
                if (Entitie.url != null && Entitie.url != "" && Entitie.url != undefined) {
                    htmlConteudo += "<img width='250' height='100' src='" + Entitie.url + "'><br/>";

                }

                htmlConteudo += "<label >" + Entitie.valor + "</label><br>";
                
                
                htmlConteudo += "<input class'btn_comprar' type='button' onclick='objetoVenda.AdicionarCarrinho(" + Entitie.id + ")' value= 'Comprar' </br>";

                htmlConteudo += " </div>";
            });
            $("#DivVenda").html(htmlConteudo);
        }
    })
};

objetoVenda.CarregarQtdCarrinho = function () {

    $("#qtdCarrinho").text("");
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











