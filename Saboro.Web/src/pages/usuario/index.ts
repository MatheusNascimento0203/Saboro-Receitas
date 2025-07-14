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
        getEditarPerfil: string;
        postEditarPerfil: string;
        excluirConta: string;
    };
}

let model: IUsuarioModel;

export function init(params: IUsuarioModel) {
    model = params;
}

export function getEditarPerfil() {
    $.get(model.urls.getEditarPerfil)
        .done(() => {
            UIkit.modal("#modal-editar-Perfil").show();
        })
        .fail((erro) => {
            Toast.error(erro);
        });
}
