import $, { data } from "jquery";
import Toast from "components/toast";
import Loading from "components/loading";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";
import formHelper from "helpers/form";

UIkit.use(Icons);

interface IUsuarioModel {
    urls: {
        index: string;
        postEditarPerfil: string;
        excluirConta: string;
        postAlterarSenha: string;
        formEditarPerfil: string;
        formAlterarSenha: string;
        deleteExcluirUsuario: string;
        indexLogin: string;
    };
}

let model: IUsuarioModel;

export function init(params: IUsuarioModel) {
    model = params;
}

export function getEditarPerfil() {
    $.get(model.urls.formEditarPerfil)
        .done(() => {
            UIkit.modal("#modal-editar-Perfil").show();
        })
        .fail((erro) => {
            Toast.error(erro);
        });
}

export function getAlterarSenha() {
        $.get(model.urls.formAlterarSenha)
        .done(() => {
            UIkit.modal("#modal-alterar-senha").show();
        })
        .fail((erro) => {
            Toast.error(erro);
        });
}

export function editarPerfil(form: HTMLFormElement, url: string) {
    const formData = formHelper.serializeObject(form);
    $.post({ url, data: formData })
        .done(() => {
            Loading.show();
            Toast.success("Perfil editado com sucesso!");
            UIkit.modal("#modal-editar-Perfil").hide();
            window.location.reload();
        })
        .fail((erro) => {
            Toast.error(erro);
        })
        .always(() => {
            UIkit.modal("#modal-editar-Perfil").hide();
            Loading.hide();
        });
}

export function alterarSenha(form: HTMLFormElement, url: string) {
    const formData = formHelper.serializeObject(form);
    $.post({ url, data: formData })
        .done(() => {
            Loading.show();
            Toast.success("Senha alterada com sucesso!");
            UIkit.modal("#modal-alterar-senha").hide();
            console.log(url);
        })
        .fail((erro) => {
            Toast.error(erro);
        })
        .always(() => {
            UIkit.modal("#modal-alterar-senha").hide();
            Loading.hide();
        });
}

export function fecharEditarPerfil() {
    UIkit.modal("#modal-editar-Perfil").hide();
    window.location.reload();
}

export function fecharAlterarSenha() {
    UIkit.modal("#modal-alterar-senha").hide();
    window.location.reload();
}

export function excluirReceita(url: string) {
    UIkit.modal(`#modal-confirmacao-exclusao-usuário`).show();
    $(`#confirmar-exclusao-usuario`)
        .off("click")
        .on("click", () => {
            Loading.show();
            $.post(url)
                .done(() => {
                    Toast.success("Usuário excluído com sucesso!");
                    UIkit.modal(
                        `#modal-confirmacao-exclusao-usuário`
                    ).hide();
                    setInterval(() => {
                    window.location.href = model.urls.indexLogin;
                    }, 3000);
                })
                .fail((erro) => {
                    Toast.error(erro);
                })
                .always(() => {
                    Loading.hide();
                });
        });

    $(`#cancelar-exclusao-usuario`)
        .off("click")
        .on("click", () => {
            UIkit.modal(`#modal-confirmacao-exclusao-usuário`).hide();
        });
}