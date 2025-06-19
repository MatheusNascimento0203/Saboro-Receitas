import $, { error } from "jquery";
import Toast from "components/toast";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";
import formHelper from "helpers/form";

UIkit.use(Icons);

interface ILoginModel {
    urls: {
        index: string;
        postRedefinirSenha: string;
        formRedefinirSenha: string;
        formCriarUsuario: string;
        postCriarUsuario: string;
    };
    message: {
        error: string;
        success: string;
    };
}

let model: ILoginModel;

export function init(params: ILoginModel) {
    model = params;
    handlerMessage(model);
}

function handlerMessage(model: ILoginModel) {
    if (model.message.error) Toast.error(model.message.error);

    if (model.message.success) Toast.success(model.message.success);
}

export function cadastrarUsuario(form: HTMLFormElement) {
    const formData = formHelper.serializeObject(form);
    $.post({ url: model.urls.postCriarUsuario, data: formData })
        .done(() => {
            Toast.success("Usuário cadastrado com sucesso!");
            setTimeout(() => {
                window.location.href = model.urls.index;
            }, 2000);
        })
        .fail((erro) => {
            Toast.error(erro);
        });
}

export function redefinirSenha(form: HTMLFormElement) {
    const formData = formHelper.serializeObject(form);
    $.post({ url: model.urls.postRedefinirSenha, data: formData })
        .done(() => {
            Toast.success("Senha Redefinida com sucesso!");
            setTimeout(() => {
                window.location.href = model.urls.index;
            }, 2000);
        })
        .fail((erro) => {
            Toast.error(erro);
        });
}