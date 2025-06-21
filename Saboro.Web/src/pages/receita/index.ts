import $ from "jquery";
import Toast from "components/toast";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";

UIkit.use(Icons);

interface IReceitaModel {
    urls: {
        index: string;
        getCadastro: string;
    };
}

let model: IReceitaModel;

export function init(params: IReceitaModel) {
    model = params;
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

    // Próximo número = quantidade atual + 1
    const numero = quantidadeLinhas + 1;

    const div = document.createElement("div");
    div.className = "uk-flex uk-flex-middle uk-margin-small linha-ingrediente";
    div.id = `ingrediente-${numero}`;

    div.innerHTML = `
        <input class="uk-input uk-border-rounded uk-width-expand" 
               type="text" 
               name="Ingredientes[]" 
               placeholder="Ingrediente ${numero}" 
               value="${valor}"/>

        <button type="button" class="uk-button uk-button-default uk-margin-small-left" 
                onclick="saboro.receita.removerIngrediente('ingrediente-${numero}')">
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

    // Conta quantas linhas existem no momento
    const quantidadeLinhas = container.querySelectorAll(".linha-passo").length;

    // Próximo número = quantidade atual + 1
    const numero = quantidadeLinhas + 1;

    const div = document.createElement("div");
    div.className = "uk-flex uk-flex-middle uk-margin-small linha-passo";
    div.id = `passo-${numero}`;

    div.innerHTML = `
    <textarea class="uk-textarea uk-border-rounded uk-width-expand" 
              name="Passos[]" 
              placeholder="Passo ${numero}" 
              style="height: 80px;">${valor}</textarea>

        <button type="button" class="uk-button uk-button-default uk-margin-small-left" 
                onclick="saboro.receita.removerPasso('passo-${numero}')">
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
