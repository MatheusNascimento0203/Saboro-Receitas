import $, { data } from "jquery";
import Toast from "components/toast";
import Loading from "components/loading";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";
import formHelper from "helpers/form";

UIkit.use(Icons);

interface IReceitaModel {
    urls: {
        index: string;
        getCadastro: string;
        postCadastrarReceita: string;
        removerReceita: string;
        getEditarReceita: string;
        postEditarReceita: string;
        homeSemReceita: string;
    };
}

let model: IReceitaModel;

export function init(params: IReceitaModel) {
    model = params;
}

export function cadastrarReceita(form: HTMLFormElement) {
    const formData = formHelper.serializeObject(form);
    $.post({ url: model.urls.postCadastrarReceita, data: formData })
        .done(() => {
            Loading.show();
            Toast.success("Receita cadastrada com sucesso!");
            setTimeout(() => {
                window.location.href = model.urls.index;
            }, 2000);
        })
        .fail((erro) => {
            Loading.hide();
            Toast.error(erro);
        })
        .always(() => {
            Loading.hide();
        });
}

export function editarReceita(form: HTMLFormElement, url: string) {
    const formData = formHelper.serializeObject(form);
    $.post({ url, data: formData })
        .done(() => {
            Loading.show();
            console.log(url);
            console.log(formData);
            Toast.success("Receita editada com sucesso!");
        })
        .fail((erro) => {
            Loading.hide();
            Toast.error(erro);
        })
        .always(() => {
            Loading.hide();
        });
}

export const getCadastroReceita = () => {
    $.get(`${model.urls.getCadastro}`, {})
        .done(() => {
            window.location.href = `${model.urls.getCadastro}`;
        })
        .fail((response) => {
            Toast.error(response.responseText);
        });
};

export function adicionarIngrediente(valor = "") {
    const container = document.getElementById("lista-ingredientes");

    // Conta quantas linhas existem no momento
    const quantidadeLinhas =
        container.querySelectorAll(".linha-ingrediente").length;

    const index = quantidadeLinhas;

    const div = document.createElement("div");
    div.className = "uk-flex uk-flex-middle uk-margin-small linha-ingrediente";
    div.id = `ingrediente-${index}`;

    div.innerHTML = `
        <input class="uk-input uk-border-rounded uk-width-expand" 
               type="text" 
               name="Ingredientes[${index}].DescricaoIngrediente" 
               placeholder="Ingrediente ${index + 1}" 
               value="${valor}"/>

        <button type="button" class="uk-margin-small-left botao-excluir-lista" 
                onclick="saboro.receita.removerIngrediente('ingrediente-${index}')">
            &times;
        </button>
    `;

    container.appendChild(div);
}

export function removerIngrediente(id) {
    const div = document.getElementById(id);
    if (div) {
        div.remove();
    }

    // Após remover, atualiza os números dos que ficaram
    atualizarNumeracaoIngredientes();
}

function atualizarNumeracaoIngredientes() {
    const container = document.getElementById("lista-ingredientes");
    const linhas = container.querySelectorAll(".linha-ingrediente");

    linhas.forEach((linha, index) => {
        const numero = index + 1;
        linha.id = `ingrediente-${numero}`;

        const input = linha.querySelector("input");
        if (input) {
            input.placeholder = `Ingrediente ${numero}`;
        }

        const botao = linha.querySelector("button");
        if (botao) {
            botao.setAttribute(
                "onclick",
                `saboro.receita.removerIngrediente('ingrediente-${numero}')`
            );
        }
    });
}

export function adicionarPassos(valor = "") {
    const container = document.getElementById("lista-passos");

    const quantidadeLinhas = container.querySelectorAll(".linha-passo").length;
    const index = quantidadeLinhas;

    const div = document.createElement("div");
    div.className = "uk-flex uk-flex-middle uk-margin-small linha-passo";
    div.id = `passo-${index}`;

    div.innerHTML = `
        <div class="uk-width-expand">
            <textarea class="uk-textarea uk-border-rounded uk-width-1-1" 
                      name="ModosPreparo[${index}].Descricao" 
                      placeholder="Passo ${index + 1}" 
                      style="height: 80px;">${valor}</textarea>
            <input type="hidden" name="ModosPreparo[${index}].Ordem" value="${
        index + 1
    }" />
        </div>

        <button type="button" class="uk-margin-small-left botao-excluir-lista uk-margin-medium-bottom" 
                onclick="saboro.receita.removerPasso('passo-${index}')">
            &times;
        </button>
    `;

    container.appendChild(div);
}

export function removerPasso(id) {
    const div = document.getElementById(id);
    if (div) {
        div.remove();
    }

    // Após remover, atualiza os números dos que ficaram
    atualizarNumeracaoPassos();
}

function atualizarNumeracaoPassos() {
    const container = document.getElementById("lista-passos");
    const linhas = container.querySelectorAll(".linha-passo");

    linhas.forEach((linha, index) => {
        const numero = index + 1;
        linha.id = `passo-${numero}`;

        const input = linha.querySelector("textarea");
        if (input) {
            input.placeholder = `Passo ${numero}`;
        }

        const botao = linha.querySelector("button");
        if (botao) {
            botao.setAttribute(
                "onclick",
                `saboro.receita.removerPasso('passo-${numero}')`
            );
        }
    });
}

export function excluirReceita(url: string, id: number) {
    UIkit.modal(`#modal-confirmacao-exclusao-receita-${id}`).show();
    $(`#confirmar-exclusao-receita-${id}`)
        .off("click")
        .on("click", () => {
            Loading.show();
            $.post(url)
                .done(() => {
                    $(`#receita-${id}`).remove();
                    Toast.success("Receita excluída com sucesso!");
                    UIkit.modal(
                        `#modal-confirmacao-exclusao-receita-${id}`
                    ).hide();
                    setTimeout(() => {
                        if (!$(".card-receitas").children().length) {
                            window.location.href = model.urls.homeSemReceita;
                        } else {
                            window.location.reload();
                        }
                    }, 1000);
                })
                .fail((erro) => {
                    Toast.error(erro);
                })
                .always(() => {
                    Loading.hide();
                });
        });

    $(`#cancelar-exclusao-receita-${id}`)
        .off("click")
        .on("click", () => {
            UIkit.modal(`#modal-confirmacao-exclusao-receita-${id}`).hide();
        });
}

export function visualizarReceita(id: number) {
    UIkit.modal(`#modal-visualizar-receita-${id}`).show();
}
